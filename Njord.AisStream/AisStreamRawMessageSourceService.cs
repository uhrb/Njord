using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Njord.Ais.MessageProcessing;
using System.Buffers;
using System.Diagnostics.Metrics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Njord.AisStream
{
    public sealed class AisStreamRawMessageSourceService : BackgroundService
    {
        private readonly ILogger<AisStreamRawMessageSourceService> _logger;
        private readonly AisStreamMessageSourceProxy _source;
        private readonly Uri _uri;
        private readonly string _apiKey;
        private readonly string[] _filterMessageTypes;
        private readonly double[][][] _boundingBoxes;
        private readonly Counter<long> _messageReceivedCounter;
        private readonly Counter<long> _exceptionCounter;
        private readonly int _reconnectDelay;

        public AisStreamRawMessageSourceService(
            ILogger<AisStreamRawMessageSourceService> logger,
            IOptions<AisStreamRawMessageSourceOptions> options,
            AisStreamMessageSourceProxy source,
            IMeterFactory meterFactory)
        {
            _logger = logger;
            _source = source;
            _uri = options.Value.Uri;
            _apiKey = options.Value.ApiKey;
            _filterMessageTypes = options.Value.FilterMessageTypes;
            _boundingBoxes = options.Value.BoundingBoxes;
            var fullName = typeof(AisStreamRawMessageSourceService).FullName!;
            var meter = meterFactory.Create(fullName, "v1.0");
            _messageReceivedCounter = meter.CreateCounter<long>($"{fullName}.messagesReceived");
            _exceptionCounter = meter.CreateCounter<long>($"{fullName}.exceptionsCount");
            _reconnectDelay = options.Value.ReconnectDelaySeconds;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (false == token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_reconnectDelay * 1000, token);
                    var connectionId = Guid.NewGuid().ToString("N");
                    using ClientWebSocket _webSocket = new();
                    _logger.LogInformation("Connecting to AIS stream");
                    await _webSocket.ConnectAsync(_uri, token);
                    var subMessage = new AisStreamSubscriptionMessage
                    {
                        Apikey = _apiKey,
                        BoundingBoxes = _boundingBoxes,
                        FilterMessageTypes = _filterMessageTypes?.Length > 0 ? _filterMessageTypes : null
                    };

                    string message = JsonSerializer.Serialize(subMessage);
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(message);
                    _logger.LogInformation("Sending message to AIS stream");
                    await _webSocket.SendAsync(new ArraySegment<byte>(bytesToSend), WebSocketMessageType.Text, true, token);
                    var buff = ArrayPool<byte>.Shared.Rent(10240);
                    try
                    {
                        while (false == token.IsCancellationRequested)
                        {
                            var result = await _webSocket.ReceiveAsync(buff, token);

                            token.ThrowIfCancellationRequested();

                            if (!result.EndOfMessage)
                            {
                                throw new NotSupportedException("Message is too large");
                            }
                            var cpBuff = new byte[result.Count];
                            buff.AsSpan(0, result.Count).CopyTo(cpBuff);
                            var newBlock = new ReadOnlyMemory<byte>(cpBuff);

                            var msg = new RawAisMessage
                            {
                                MessageFormat = "json/aisstream",
                                RawData = newBlock
                            };
                            _messageReceivedCounter.Add(1, [new ("connection", connectionId)]);
                            await _source.ReceiveAsync(msg, token).ConfigureAwait(false);
                        }
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buff);
                    }
                }
                catch (Exception ex)
                {
                    if (ex is OperationCanceledException)
                    {
                        _logger.LogInformation("AIS service stopped");
                        return;
                    }
                    _exceptionCounter.Add(1);
                    _logger.LogError(ex, "AIS service restarting in {_reconnectDelay} seconds", _reconnectDelay);
                }
            }
        }
    }
}
