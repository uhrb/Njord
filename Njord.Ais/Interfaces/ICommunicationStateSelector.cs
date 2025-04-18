namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// Communication state selector interface
    /// </summary>
    public interface ICommunicationStateSelector
    {
        /// <summary>
        /// Communication state selector flag
        /// </summary>
        public bool IsCommunicationStateITDMA{ get; init; }
    }
}
