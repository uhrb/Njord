namespace Njord.Ais.Interfaces
{
    public interface ICallSign
    {
        /// <summary>
        /// 7 x 6 bit ASCII characters, @@@@@@@ = not available = default.
        /// Craft associated with a parent vessel, should use “A” followed by the last
        /// 6 digits of the MMSI of the parent vessel.Examples of these craft include
        /// towed vessels, rescue boats, tenders, lifeboats and liferafts.
        /// </summary>
        public string CallSign { get; init; }
    }
}
