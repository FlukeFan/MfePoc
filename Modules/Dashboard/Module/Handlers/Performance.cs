using System.Threading.Tasks;
using MfePoc.Generation.Contract;
using MfePoc.Shared.Bus;
using Microsoft.AspNetCore.SignalR;

namespace MfePoc.Dashboard.Handlers
{
    public class Performance : IHandle<OnPerfSent>
    {
        private static bool _counting = false;
        private static int _count;

        private readonly IHubContext<DashboardHub> _hub;

        public Performance(IHubContext<DashboardHub> hub)
        {
            _hub = hub;
        }

        public async Task HandleAsync(OnPerfSent message)
        {
            if (!_counting)
            {
                _count = 1;
                _counting = true;
            }
            else
            {
                _count++;
                return;
            }

            await Task.Delay(1000);
            await _hub.Clients.All.SendAsync("notify", $"Counted {_count} test messages per second");

            _counting = false;
        }
    }
}
