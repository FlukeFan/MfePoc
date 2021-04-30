using System.Threading.Tasks;
using MfePoc.Generation.Contract;
using MfePoc.Shared.Bus;
using Microsoft.AspNetCore.SignalR;

namespace MfePoc.Dashboard.Handlers
{
    public class MessageNotifier :
        IHandle<OnStockUpdated>,
        IHandle<OnServiceStarted>,
        IHandle<OnStockConsumed>
    {
        private readonly IHubContext<DashboardHub> _hub;

        public MessageNotifier(IHubContext<DashboardHub> hub)
        {
            _hub = hub;
        }

        public async Task HandleAsync(OnStockUpdated message)
        {
            await _hub.Clients.All.SendAsync("notify", $"Stock: Red={message.Red} Green={message.Green} Blue={message.Blue}");
        }

        public async Task HandleAsync(OnServiceStarted message)
        {
            await _hub.Clients.All.SendAsync("notify", $"{message.Name} Service Started");
        }

        public async Task HandleAsync(OnStockConsumed message)
        {
            await _hub.Clients.All.SendAsync("notify", $"Consumed: Red={message.Red} Green={message.Green} Blue={message.Blue}");
        }
    }
}
