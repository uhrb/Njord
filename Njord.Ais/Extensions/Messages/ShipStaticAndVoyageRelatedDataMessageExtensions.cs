using Njord.Ais.Enums;
using Njord.Ais.Extensions.Types;
using Njord.Ais.Messages;

namespace Njord.Ais.Extensions.Messages
{
    public static class ShipStaticAndVoyageRelatedDataMessageExtensions
    {

        public static bool IsValid(this IShipStaticAndVoyageRelatedDataMessage message)
        {
            return message.UserId.IsValidMMSI()
                && message.MessageId == AisMessageType.ShipStaticAndVoyageRelatedData
                && message.IsIMOValidRange();

        }


        /// <summary>
        /// Indicates if the ship IMO Valid
        /// <code>
        /// ╒═══════════════════════════════════════════╤═══════════════════════╕
        /// │ Name                                      │ Value                 │
        /// ╞═══════════════════════════════════════════╪═══════════════════════╡
        /// │ Default                                   │ 0                     │
        /// ├───────────────────────────────────────────┼───────────────────────┤
        /// │ Not used                                  │ 0000000001-0000999999 │
        /// ├───────────────────────────────────────────┼───────────────────────┤
        /// │ Valid number                              │ 0001000000-0009999999 │
        /// ├───────────────────────────────────────────┼───────────────────────┤
        /// │ official flag state number                │ 0010000000-1073741823 │
        /// └───────────────────────────────────────────┴───────────────────────┘
        /// </code>
        /// </summary>
        /// <param name="data"></param>
        public static bool IsIMOValidRange(this IShipStaticAndVoyageRelatedDataMessage data)
        {
            return (data.IMONumber >= 1000000 && data.IMONumber <= 1073741823) || data.IMONumber == 0;
        }

        /// <summary>
        /// Indicates if the ship IMO Valid
        /// <code>
        /// ╒═══════════════════════════════════════════╤═══════════════════════╕
        /// │ Name                                      │ Value                 │
        /// ╞═══════════════════════════════════════════╪═══════════════════════╡
        /// │ Default                                   │ 0                     │
        /// ├───────────────────────────────────────────┼───────────────────────┤
        /// │ Not used                                  │ 0000000001-0000999999 │
        /// ├───────────────────────────────────────────┼───────────────────────┤
        /// │ Valid number                              │ 0001000000-0009999999 │
        /// ├───────────────────────────────────────────┼───────────────────────┤
        /// │ official flag state number                │ 0010000000-1073741823 │
        /// └───────────────────────────────────────────┴───────────────────────┘
        /// </code>
        /// </summary>
        /// <param name="data"></param>
        public static bool IsIMOFlagStateNumber(this IShipStaticAndVoyageRelatedDataMessage data)
        {
            return data.IMONumber >= 10000000 && data.IMONumber <= 1073741823;
        }

        /// <summary>
        /// Checks either Name is formed according to recommendation for SAR Aircraft
        /// </summary>
        /// <param name="data">Date report to check on</param>
        /// <returns>True or false</returns>
        public static bool IsSARAircraftName(this IShipStaticAndVoyageRelatedDataMessage data)
        {
            if (data.Name.IsUnavailiableOrNullEmpty())
            {
                return false;
            }
            return data.Name.StartsWith("SAR AIRCRAFT");
        }
    }
}
