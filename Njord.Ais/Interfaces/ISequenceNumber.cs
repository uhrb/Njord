namespace Njord.Ais.Interfaces
{
    public interface ISequenceNumber
    {
        /// <summary>
        /// The sequence number should be assigned in the appropriate presentation interface message which is
        /// input to the station. The destination station should return the same sequence number in 
        /// its acknowledgement message on the presentation interface. The source station should not reuse 
        /// a sequence number until it has been acknowledged or time-out has occurred.
        /// </summary>
        public byte SequenceNumber { get; init; }
    }
}
