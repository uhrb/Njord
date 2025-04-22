using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Njord.Ais.MessageProcessing;
using System.Buffers;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Sockets;

namespace Njord.NCA
{
    public class NcaStreamRawMessageSourceService : BackgroundService
    {
        private readonly ILogger<NcaStreamRawMessageSourceService> _logger;
        private readonly NcaStreamRawMessageSourceProxy _source;
        private readonly NcaStreamRawMessageSourceOptions _options;
        private readonly Counter<long> _messageReceivedCounter;

        public NcaStreamRawMessageSourceService(
            ILogger<NcaStreamRawMessageSourceService> logger,
            IOptions<NcaStreamRawMessageSourceOptions> options,
            NcaStreamRawMessageSourceProxy source,
            IMeterFactory meterFactory
            )
        {
            _logger = logger;
            _source = source;
            _options = options.Value;
            var fullName = typeof(NcaStreamRawMessageSourceService).FullName!;
            var meter = meterFactory.Create(fullName, "v1.0");
            _messageReceivedCounter = meter.CreateCounter<long>($"{fullName}.messagesReceived");
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (false == token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_options.ReconnectDelaySeconds*1000, token);
                    var connectionId = Guid.NewGuid().ToString("N");
                    var buff = ArrayPool<byte>.Shared.Rent(1024);
                    try
                    {
                        using (var client = new TcpClient())
                        {
                            var ipEndPoint = new IPEndPoint(IPAddress.Parse(_options.ServerIP), _options.Port);
                            _logger.LogInformation("Connecting to NCA AIS stream");
                            await client.ConnectAsync(ipEndPoint);
                            using (var stream = client.GetStream())
                            {
                                _logger.LogInformation("Reading from NCA AIS stream");
                                var lineBuffer = new List<byte>();
                                int bytesRead = 0;
                                while ((bytesRead = await stream.ReadAsync(buff, token)) > 0)
                                {
                                    token.ThrowIfCancellationRequested();
                                    int i = 0;
                                    while (i < bytesRead)
                                    {
                                        if (buff[i] == 0x0D) // CR, possibly LF
                                        {
                                            var rawMessage = lineBuffer.ToArray();
                                            var msg = new RawAisMessage
                                            {
                                                MessageFormat = "nmea/nca",
                                                RawData = rawMessage
                                            };
                                            _messageReceivedCounter.Add(1, [new("connection", connectionId)]);
                                            await _source.ReceiveAsync(msg, token).ConfigureAwait(false);
                                            lineBuffer.Clear(); 
                                        }
                                        else if(buff[i] != 0x0A)  // possible LF
                                        {
                                            lineBuffer.Add(buff[i]);
                                        }
                                        i++;
                                    }
                                }
                            }
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
                        _logger.LogInformation("NCA AIS service stopped");
                        return;
                    }
                    _logger.LogError(ex, "NCA AIS service restarting in {ReconnectDelaySeconds} seconds", _options.ReconnectDelaySeconds);
                }
            }
        }
    }
}
