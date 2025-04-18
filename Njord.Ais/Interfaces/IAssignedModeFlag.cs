using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    public interface IAssignedModeFlag
    {
        /// <summary>
        /// Class B assigned mode flag. True if in assigned mode
        /// </summary>
        public bool IsInAssignedMode { get; init; }
    }
}
