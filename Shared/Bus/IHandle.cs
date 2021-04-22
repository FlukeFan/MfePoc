using System.Threading.Tasks;

namespace MfePoc.Shared.Bus
{
    public interface IHandle<TMessage>
    {
        Task HandleAsync(TMessage message);
    }
}
