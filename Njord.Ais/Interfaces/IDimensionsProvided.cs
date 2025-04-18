namespace Njord.Ais.Interfaces
{
    public interface IDimensionsProvided
    {

        /// <summary>
        /// Dimension reference to position. See <see cref="Records.Dimensions"/>
        /// Reference point for reported position.
        /// Also indicates the dimension of ship(m) (see Fig. 41 and § 3.3.3)
        /// For SAR aircraft, the use of this field may be decided by the responsible
        /// administration. If used it should indicate the maximum dimensions of the
        /// craft.As default should A = B = C = D be set to “0”
        /// </summary>
        public IDimensions Dimensions { get; init; }
    }
}
