namespace Njord.Ais.Interfaces
{
    public interface IName
    {
        /// <summary>
        /// Maximum 20 characters 6 bit ASCII, as defined in Table 47
        /// “@@@@@@@@@@@@@@@@@@@@” = not available = default.
        /// The Name should be as shown on the station radio license.For SAR aircraft,
        /// it should be set to “SAR AIRCRAFT NNNNNNN” where NNNNNNN
        /// equals the aircraft registration number.
        /// </summary>

        public string Name { get; init; }
    }
}
