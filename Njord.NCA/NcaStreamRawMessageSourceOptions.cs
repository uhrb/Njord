namespace Njord.NCA
{
    public record NcaStreamRawMessageSourceOptions
    {
        public required string ServerIP { get; init; }
        public required int Port { get; init; }

        public required int ReconnectDelaySeconds { get; init; }
    }
}