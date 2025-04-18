using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    public interface IDataTerminalEquipment
    {
        /// <summary>
        /// True if DTE is avail and ready, false otherwise
        /// </summary>
        public bool IsDataTerminalEquipmentAvailiable { get; init; }
    }
}
