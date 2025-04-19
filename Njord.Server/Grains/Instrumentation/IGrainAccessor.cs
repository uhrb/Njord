using Njord.Server.Grains.Interfaces;

namespace Njord.Server.Grains.Instrumentation
{
    public interface IGrainAccessor
    {
        IUnknownMaritimeEntity? GetGrainFromMMSI(string key);
    }
}