using Njord.Ais.Enums;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Messages
{
    public interface IAidsToNavigationReportMessage :
        IUserId,
        IMessageId,
        IRepeatIndicator,
        IName,
        ILongitudeAndLatitude,
        IPositionAccuracyAndRAIMFlag,
        IDimensionsProvided,
        IPositionFixingDeviceInfo,
        ITimestamp,
        IAssignedModeFlag
    {
        /// <summary>
        /// Type of AtoN. See <see cref="AidsToNavigationType"/> for details
        /// </summary>
        public AidsToNavigationType TypeOfAidsToNavigation { get; init; }

        /// <summary>
        /// AtoN off position indicator. True if off-position
        /// NOTE 1 – This flag should only be considered valid by receiving station,
        /// if the AtoN is a floating aid, and if time stamp is equal to or below 59.
        /// For floating AtoN the guard zone parameters should be set on installation
        /// </summary>
        public bool IsOffPosition { get; init; }

        /// <summary>
        /// AtoN status. See <see cref="IAidsToNavigationState"/> for details
        /// </summary>
        public IAidsToNavigationState AtoNStatus { get; init; }

        /// <summary>
        /// True, if device is virtual
        /// </summary>
        public bool IsVirtualDevice { get; init; }

        /// <summary>
        /// Name extension for AtoN
        /// </summary>
        public string NameExtension { get; init; }
    }
}
