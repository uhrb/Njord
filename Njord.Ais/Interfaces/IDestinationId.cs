namespace Njord.Ais.Interfaces
{
    public interface IDestinationId
    {
        /// <summary>
        /// Destination MMSI. See <see cref="IUserIdAndRepeat.UserId"/> for details
        /// </summary>
        public string DestinationId { get; init; }
    }
}
