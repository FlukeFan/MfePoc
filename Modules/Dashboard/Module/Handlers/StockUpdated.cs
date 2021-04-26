using System.Threading.Tasks;
using MfePoc.Generation.Contract;
using MfePoc.Shared.Bus;
using Microsoft.AspNetCore.SignalR;

namespace MfePoc.Dashboard.Handlers
{
    public class StockUpdated : IHandle<OnStockUpdated>, IHandle<OnServiceStarted>
    {
        private readonly IHubContext<DashboardHub> _hub;

        public StockUpdated(IHubContext<DashboardHub> hub)
        {
            _hub = hub;
        }

        public async Task HandleAsync(OnStockUpdated message)
        {
            await _hub.Clients.All.SendAsync("notify", $"Stock: Red={message.Red} Green={message.Green} Blue={message.Blue}");
        }

        public async Task HandleAsync(OnServiceStarted message)
        {
            await _hub.Clients.All.SendAsync("notify", $" {message.Name} Service Started");
        }
    }
}
