namespace Njord.Ais.Interfaces
{
    public interface ITrueHeading
    {
        /// <summary>
        /// Degrees (0-359) (511 indicates not available = default)
        /// </summary>
        public int TrueHeading { get; init; }
    }
}
