using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    /// <summary>
    /// Assignment should be transmitted by a base station when operating as a controlling entity. Other
    /// stations can be assigned a transmission schedule, other than the currently used one.If a station is
    /// assigned a schedule, it will also enter assigned mode. Two stations can be assigned simultaneously.
    /// </summary>
    public interface IAssignedModeCommandMessage : IUserId, IMessageId, IRepeatIndicator
    {
        /// <summary>
        /// List of assigned destinations
        /// </summary>
        public IEnumerable<IAssignedDestination> AssignedDestinations { get; init; }
    }
}
