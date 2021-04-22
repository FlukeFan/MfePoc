using System.Threading.Tasks;

namespace MfePoc.Shared.Bus
{
    public interface IBus
    {
        Task PublishAsync<T>(T message);
    }
}
