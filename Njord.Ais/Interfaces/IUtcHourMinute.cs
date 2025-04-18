namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// Utility sealed record to indicate Utc Hour and Minute
    /// </summary>
    public interface IUtcHourMinute
    {
        /// <summary>
        /// Hour; 0-23; 24 = not available = default
        /// </summary>
        public byte Hour { get; init; }

        /// <summary>
        /// Minute; 0-59; 60 = not available = default
        /// </summary>
        public byte Minute { get; init; }
    }
}
