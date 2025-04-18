namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// 16-bit application identifier (AI = DAC + FI), consisting of:
    /// – 10-bit designated area code(DAC) – based on the MID;
    /// – 6-bit function identifier(FI) – allows for 64 unique application specific messages.
    /// Each unique combination of application identifier (AI) and application data forms a functional
    /// message.The coding and decoding of the data content of a binary message is based on a table
    /// identified by the AI value.Tables identified by an International AI(IAI) value should be maintained
    /// and published by the international authority responsible for defining international function messages
    /// (IFM). Maintenance and publication of regional AI tables(RAI), defining regional function messages
    /// (RFM) should be the responsibility of national or regional authorities.
    /// </summary>
    public interface IApplicationIdentifier
    {
        /// <summary>
        /// Designated area code
        /// international (DAC = 1-9), maintained by international agreement for global use;
        /// – regional(DAC ≥ 10), maintained by the regional authorities affected;
        /// – test(DAC = 0), used for test purposes.
        /// It is recommended that DAC 2-9 be used to identify subsequent versions of international specific
        /// messages and that the administrator of application specific messages base the DAC selection on the
        /// maritime identification digit(MID) of the administrator’s country or region.It is the intention that
        /// any application specific message can be utilized worldwide.The choice of the DAC does not limit
        /// the area where the message can be used.
        /// </summary>
        public ushort DesignatedAreaCode { get; init; }

        /// <summary>
        /// The FI is a 6-bit number assigned to uniquely identify the data content structure within an application
        /// under a DAC assignment.Each DAC can support up to 64 applications.
        /// </summary>
        public byte FunctionIdentifier { get; init; }
    }
}
