using Njord.Ais.Interfaces;
using Orleans;

namespace Njord.Server.Grains.Interfaces
{
    [Alias("Njord.Server.Grains.Interfaces.IUnknownDevice")]
    public interface IUnknownMaritimeEntity : IGrainWithStringKey
    {
        Task ProcessMessage(IMessageId message);
    }
}
