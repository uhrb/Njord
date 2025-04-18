namespace Njord.Ais.Enums
{
    /// <summary>
    /// <para>Sync state</para>
    /// <code>
    /// ╒═════════════════════════════════════════════════════════╤═══════════════════════════════╕
    /// │ State                                                   │ Value                         │
    /// ╞═════════════════════════════════════════════════════════╪═══════════════════════════════╡
    /// │ A station, which has direct access to coordinated       │                               │
    /// │ universal time (UTC) timing with the required           │ UTCDirect = 0                 │  
    /// │ accuracy should indicate this by setting its            │                               │
    /// │ synchronization state to UTC direct.                    │                               │
    /// ├─────────────────────────────────────────────────────────┼───────────────────────────────┤
    /// │ A station, which is unable to get direct access to UTC, │                               │
    /// │ but can receive other stations that indicate            │ UTCIndirect = 1               │
    /// │ UTC direct, should synchronize to those stations.       │                               │
    /// │ It should then change its synchronization state to      │                               │
    /// │ UTC indirect.Only one level of UTC indirect             │                               │
    /// │ synchronization is allowed.                             │                               │
    /// ├─────────────────────────────────────────────────────────┼───────────────────────────────┤
    /// │ Mobile stations, which are unable to attain direct or   │                               │
    /// │ indirect UTC synchronization, but are able to receive   │ SynchronizedToBaseStation = 2 │ 
    /// │ transmissions from base stations, should synchronize    │                               │
    /// │ to the base station which indicates the highest number  │                               │
    /// │ of received stations, provided that two reports have    │                               │
    /// │ been received from that station in the last 40 s.       │                               │
    /// ├─────────────────────────────────────────────────────────┼───────────────────────────────┤
    /// │ A station, which is unable to attain UTC direct or UTC  │                               │
    /// │ indirect synchronization and is also unable to receive  │ SynchronizedWithOther = 3     │
    /// │  transmissions from a base station, should synchronize  │                               │
    /// │  to the station indicating the highest number of other  │                               │
    /// │  stations received during the last nine frames,         │                               │
    /// │  provided that two reports have been received from      │                               │
    /// │  that station in the last 40 s.                         │                               │
    /// └─────────────────────────────────────────────────────────┴───────────────────────────────┘
    /// </code>
    /// </summary>
    public enum CommunicationSyncState : byte
    {
        /// <summary>
        /// Have direct access to UTC 
        /// </summary>
        UTCDirect = 0,

        /// <summary>
        /// Unable to get direct access to UTC, but can receive other stations that indicate UTC direct
        /// </summary>
        UTCIndirect = 1,

        /// <summary>
        /// Unable to attain direct or indirect UTC synchronization, but are able to receive transmissions from base stations
        /// </summary>
        SynchronizedToBaseStation = 2,

        /// <summary>
        /// Unable to attain UTC direct or indirect synchronization and is also unable to receive transmissions from a base station, but syncronized with other stations
        /// </summary>
        SynchronizedWithOther = 3,
    }
}
