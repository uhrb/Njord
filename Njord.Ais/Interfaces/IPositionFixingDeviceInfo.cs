using Njord.Ais.Enums;

namespace Njord.Ais.Interfaces
{
    public interface IPositionFixingDeviceInfo
    {
        /// <summary>
        /// Device type used to identify precise position <see cref="PositionFixingDeviceType"/>
        /// </summary>
        public PositionFixingDeviceType FixingDeviceType { get; init; }
    }
}
