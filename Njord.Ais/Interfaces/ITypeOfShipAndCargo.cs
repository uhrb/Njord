using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    public interface ITypeOfShipAndCargo
    {
        /// <summary>
        /// Type of ship and cargo type. See <see cref="Enums.TypeOfShipAndCargoType"/>
        /// </summary>
        public TypeOfShipAndCargoType TypeOfShipAndCargoType { get; init; }

    }
}
