namespace Njord.Ais.Interfaces
{
    public interface IUserId
    {
        /// <summary>
        /// <para>
        /// Unique identifier such as MMSI number according to table below.
        /// The MID represents the administration having jurisdiction over the call identity for the navigational aid
        /// </para>
        /// <para>
        /// X - numbers from 0 to 9
        /// </para>
        /// <para>
        /// Y - numbers from 0 to 9, when used together with XX - XX is manufacturer ID, 00 Manufacturer id for test
        /// </para>
        /// <code>
        /// ╒═══════════════════════════════════╤═══════════════════════════════════════════════╕
        /// │ Template                          │ Purpose                                       │ 
        /// ╞═══════════════════════════════════╪═══════════════════════════════════════════════╡
        /// │ MIDXXXXXX                         │ General Format for ships                      │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 0MIDXXXXX                         │ Group ship stations (group broadcast)         │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 00MID1XXX                         │ Coast stations                                │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 00MID2XXX                         │ Port stations(harbour radio stations)         │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 00MID3XXX                         │ Pilot stations                                │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 00MID4XXX                         │ AIS repeater stations                         │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 00MID5XXX                         │ AIS base stations (VDL controlling stations)  │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 00MIDXXXX                         │ Group coast stations                          │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 00MID0000                         │ Reserved for a Group Coast Station Identity   │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 009990000                         │ Reserved for VFH stations 00XXXXXXX           │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 111MID1XX                         │ Fixed wing Aircraft AIS identifiers           │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 111MID5XX                         │ Helicopters AIS identifiers                   │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 111MID000                         │ Reserved for Aircraft group identity          │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 99MIDXXXX                         │ General Aids to Navigation                    │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 99MID1XXX                         │ Physical AIS AtoN                             │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 99MID6XXX                         │ Virtual AIS AtoN                              │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 99MID8XXX                         │ Mobile AIS AtoN                               │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 98MIDXXXX                         │ General craft associated with parent ship     │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 8MIDXXXXX                         │ General VHF transceiver with DSC and integral │
        /// │                                   │ GNSS receiver                                 │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 970XXYYYY                         │ Automatic identification system-search        │
        /// │                                   │ and rescue transmitter                        │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 972XXYYYY                         │ Man overboard                                 │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 974XXYYYY                         │ Emergency position-indicating radio           │
        /// │                                   │ beacon-automatic identification system        │
        /// ├───────────────────────────────────┼───────────────────────────────────────────────┤
        /// │ 979YYYYYY                         │ Autonomous maritime radio devices Group B     │
        /// └───────────────────────────────────┴───────────────────────────────────────────────┘
        /// </code>
        /// </summary>
        public string UserId { get; init; }
    }
}
