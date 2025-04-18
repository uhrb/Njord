namespace Njord.Ais.Interfaces
{
    /// <summary>
    /// Dimension reference for the ship
    /// The dimension A should be in the direction of the transmitted
    /// heading information(bow)
    /// Reference point of reported position not available, but
    /// dimensions of ship are available: A = C = 0 and B 0 and D 0.
    /// Neither reference point of reported position nor dimensions of
    /// ship available; A = B = C = D = 0 (= default).
    /// For use in the message table, A = most significant field,
    /// D = least significant field.
    /// <code>
    ///      /\       -|
    ///     /  \       |
    ///    /    \      |
    ///   /      \     |
    ///   |      |     |   A
    ///   |      |     |
    ///   |      |     |
    ///   |      |     |
    ///   |      |     |
    ///   |  O   |    -|
    ///   |      |     |   B
    ///   |      |     |
    ///   --------    -|
    ///   
    ///   |  |   |
    ///   --------
    ///    C  D
    /// </code>
    /// </summary>
    public interface IDimensions
    {
        /// <summary>
        /// The dimension A should be in the direction to bow
        /// </summary>
        public ushort A { get; init; }

        /// <summary>
        /// The dimension B should be in the direction to astern 
        /// </summary>
        public ushort B { get; init; }

        /// <summary>
        /// The dimension C should be in the direction to port
        /// </summary>
        public byte C { get; init; }

        /// <summary>
        ///  The dimension D should be in the direction to starboard
        /// </summary>
        public byte D { get; init; }

    }
}
