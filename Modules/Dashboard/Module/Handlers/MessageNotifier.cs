using System.Threading.Tasks;
using MfePoc.Shared;
using MfePoc.Shared.Bus;
using Microsoft.AspNetCore.SignalR;

namespace MfePoc.Dashboard.Handlers
{
    public class MessageNotifier :
        IHandle<OnServiceStarted>,
        IHandle<Generation.Contract.OnStockUpdated>,
        IHandle<Generation.Contract.OnStockConsumed>,
        IHandle<Mixing.Contract.OnStockUpdated>,
        IHandle<Mixing.Contract.OnStockConsumed>,
        IHandle<Sales.Contract.OnSellExecuted>
    {
        private readonly IHubContext<DashboardHub> _hub;

        public MessageNotifier(IHubContext<DashboardHub> hub)
        {
            _hub = hub;
        }

        public async Task HandleAsync(OnServiceStarted message)
        {
            await _hub.Clients.All.SendAsync("notify", $"{message.Name} Service Started");
        }

        public async Task HandleAsync(Generation.Contract.OnStockUpdated message)
        {
            await _hub.Clients.All.SendAsync("notify", $"Stock: Red={message.Red} Green={message.Green} Blue={message.Blue}");
        }

        public async Task HandleAsync(Generation.Contract.OnStockConsumed message)
        {
            await _hub.Clients.All.SendAsync("notify", $"Consumed: Red={message.Red} Green={message.Green} Blue={message.Blue}");
        }

        public async Task HandleAsync(Mixing.Contract.OnStockUpdated message)
        {
            await _hub.Clients.All.SendAsync("notify", $"Stock: Yellow={message.Yellow} Cyan={message.Cyan} Magenta={message.Magenta}");
        }

        public async Task HandleAsync(Mixing.Contract.OnStockConsumed message)
        {
            await _hub.Clients.All.SendAsync("notify", $"Consumed: Yellow={message.Yellow} Cyan={message.Cyan} Magenta={message.Magenta}");
        }

        public async Task HandleAsync(Sales.Contract.OnSellExecuted message)
        {
            await _hub.Clients.All.SendAsync("notify", $"Sold '{message.Description}' for {message.Amount.FormatGBP()} at {message.WhenUtc}");
        }
    }
}
