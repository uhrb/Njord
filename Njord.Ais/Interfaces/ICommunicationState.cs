namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// Communication state related message
    /// </summary>
    public interface ICommunicationState 
    {
        /// <summary>
        /// <para>Communication state.</para> 
        /// SOTDMA for messages 1,2,4,11
        /// ITDMA for messages 3
        /// </summary>
        public uint CommunicationState { get; init; }
    }
}
