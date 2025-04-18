using Njord.Ais.Extensions.Types;
using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class AssignedDestinationExtensions 
    {
        public static bool IsValid(this IAssignedDestination dest)
        {
            var val = dest.DestinationId.IsValidMMSI()
                && dest.Offset < 4096
                && dest.Offset > 1
                && dest.Increment <= 7; 

            return val;
        }


        public static bool IsDisregardAssignment(this IAssignedDestination dest)
        {
            return dest.IsValid() && dest.Increment == 7 && dest.Offset == 0;
        }


        /// <summary>
        /// Checks if slots offset assigned and returns number of slots
        /// </summary>
        /// <param name="dest">Destination/param>
        /// <param name="slotOffset">real slot offset according to the standard</param>
        /// <returns>True if slot assignment done</returns>
        public static bool IsSlotsOffsetAssigned(this IAssignedDestination dest, out ushort? slotOffset)
        {

            slotOffset = null;
            if(dest.Increment == 0 || !dest.IsValid())
            {
                return false;
            }

            slotOffset = dest.Increment switch
            {
                var a when a == 1 => 1125,
                var a when a == 2 => 375,
                var a when a == 3 => 225,
                var a when a == 4 => 125,
                var a when a == 5 => 75,
                var a when a == 6 => 45,
                _ => null
            };

            // invalid or disregard asignment
            if(slotOffset == null)
            {
                return false;
            }

            return true;
        }



        /// <summary>
        /// Reports if report rate requested instead of offset. It happens when Increment is set to zero
        /// </summary>
        /// <param name="dest">Assigned destination</param>
        /// <param name="reportsPerMinute">
        /// Reports per minute. Returns maximum 600, 
        /// whenever the values is according to standard. If less than 20, station should use 20. 
        /// If not 20*X, than station should use closest to 20*X
        /// </param>
        /// <returns>Is requested report per minute or usual asignment</returns>
        public static bool IsReportRateAssigned(this IAssignedDestination dest, out ushort? reportsPerMinute)
        {
            reportsPerMinute = null;

            if (dest.Increment != 0 || dest.Offset == 0 || !dest.IsValid())
            {
                return false;
            }
            reportsPerMinute = dest.Offset > 600 ? (ushort)600 : dest.Offset;
            return true;
        }
    }
}
