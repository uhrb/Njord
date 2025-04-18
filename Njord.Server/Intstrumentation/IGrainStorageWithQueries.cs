using Orleans.Storage;

namespace Njord.Server.Intstrumentation
{
    public interface IGrainStorageWithQueries : IGrainStorage
    {
        public IEnumerable<GrainStateStored> GetAllStatesByGrainType(string grainType);
    }
}
