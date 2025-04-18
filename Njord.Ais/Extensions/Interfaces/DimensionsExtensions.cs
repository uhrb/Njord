using Njord.Ais.Interfaces;

namespace Njord.Ais.Extensions.Interfaces
{
    public static class DimensionsExtensions
    {
        /// <summary>
        /// Get the vessel length from the dimension reference
        /// </summary>
        /// <param name="dim">dimension reference</param>
        /// <returns>Vessel total length or null if not available</returns>
        public static ushort? GetVesselLength(this IDimensionsProvided dim)
        {
            if(dim.Dimensions.A == 0 && dim.Dimensions.B == 0 && dim.Dimensions.C == 0 && dim.Dimensions.D == 0)
            {
                return null;
            }

            return (ushort?)(dim.Dimensions.A + dim.Dimensions.B);

        }

        /// <summary>
        /// Get the vessel width from the dimension reference
        /// </summary>
        /// <param name="dim">dimension reference</param>
        /// <returns>Vessel total width or null if not availiable</returns>
        public static byte? GetVesselWidth(this IDimensionsProvided dim)
        {
            if (dim.Dimensions.A == 0 && dim.Dimensions.B == 0 && dim.Dimensions.C == 0 && dim.Dimensions.D == 0)
            {
                return null;
            }

            return (byte?)(dim.Dimensions.C + dim.Dimensions.D);
        }
    }
}
