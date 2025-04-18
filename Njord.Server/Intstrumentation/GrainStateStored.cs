using Njord.Server.Grains.States.Abstracts;

namespace Njord.Server.Intstrumentation
{
    public record GrainStateStored
    {
        public required string GrainType { get; init; }
        public required string GrainId { get; init; }   
        public required AbstractPositionState GrainState { get; init; }
    }
}
