using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    public interface INavigationalStatus
    {
        /// <summary>
        /// Current navigational status. See <see cref="Enums.NavigationalStatus"/> for details
        /// </summary>
        public NavigationalStatus NavigationalStatus { get; init; }
    }
}
