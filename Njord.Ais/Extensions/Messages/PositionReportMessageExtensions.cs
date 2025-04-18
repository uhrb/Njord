using Njord.Ais.Enums;
using Njord.Ais.Extensions.Interfaces;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    /// <summary>
    /// Extenstions for <see cref="Messages.IPositionReportMessage"/>
    /// </summary>
    public static class PositionReportMessageExtensions
    {
        /// <summary>
        /// CHeck if message is valid. DO not check communication state, since its depends on message typ
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool IsValid(this IPositionReportMessage message)
        {
            var value = message.IsRateOfTurnValidRange()
                && message.IsSpeedOverGroundValidRange()
                && message.IsCourseOverGroundValidRange()
                && message.UserId.IsValidMMSI()
                && message.IsLatitudeAndLongitudeValidRange()
                && message.IsTrueHeadingValidRange()
                && (
                    message.MessageId == AisMessageType.PositionReportScheduled
                    || message.MessageId == AisMessageType.PositionReportAssignedScheduled
                    || message.MessageId == AisMessageType.PositionReportSpecial
                );

            return value;
        }

        public static bool IsRateOfTurnValidRange(this IPositionReportMessage message)
        {
            return message.RateOfTurn >= -128 && message.RateOfTurn <= 127;
        }

        /// <summary>
        /// Checks if the rate of turn value is available.
        /// </summary>
        /// <param name="report">The position report.</param>
        /// <returns>True if rate of turn is available, otherwise false.</returns>
        public static bool IsRateOfTurnAvailiable(this IPositionReportMessage report)
        {
            return report.RateOfTurn != -128;
        }
    }
}
