namespace Njord.Ais.Enums
{
    /// <summary>
    /// TX/RX mode
    /// </summary>
    public enum TxRxModeType : byte
    {
        /// <summary>
        /// TX Channel A, TX Channel B, RX Channel A, RX Channel B
        /// </summary>
        TXChannelA_TXChannelB_RXChannelA_RXChannelB = 0,

        /// <summary>
        /// TX Channel A, RX Channel A, RX Channel B
        /// </summary>
        TXChannelA_RXChannelA_RXChannelB = 1,

        /// <summary>
        /// TX Channel B, RX Channel A, RX Channel B
        /// </summary>
        TXChannelB_RXChannelA_RXChannelB = 2,
    }
}
