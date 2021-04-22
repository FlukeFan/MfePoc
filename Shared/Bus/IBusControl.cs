using System.Threading.Tasks;

namespace MfePoc.Shared.Bus
{
    public interface IBusControl
    {
        Task StartAsync(string name);
    }
}
