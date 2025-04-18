using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    public interface IRetransmitFlag
    {
        /// <summary>
        /// Retransmit flag. True if retransmitted
        /// </summary>
        public bool IsRetransmitted { get; init; }

    }
}
