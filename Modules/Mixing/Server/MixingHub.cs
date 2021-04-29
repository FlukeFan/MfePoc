using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MfePoc.Mixing.Client.Comms;
using Microsoft.AspNetCore.SignalR;

namespace MfePoc.Mixing.Server
{
    public class MixingHub : Hub, IRequests
    {
        private readonly StockDb _stockDb;

        public MixingHub(StockDb stockDb)
        {
            _stockDb = stockDb;
        }

        public string RequestHostDetail()
        {
            return $"This response was from process Id={Process.GetCurrentProcess().Id} Framework={RuntimeInformation.FrameworkDescription}";
        }

        public StockLevelResponse RequestStockLevels()
        {
            return new StockLevelResponse
            {
                Yellow = _stockDb.Yellow,
                Cyan = _stockDb.Cyan,
                Magenta = _stockDb.Magenta,
            };
        }

        public async Task<string> RequestMixAsync(int red, int green, int blue)
        {
            var message = _stockDb.Mix(red, green, blue);
            await Clients.All.SendAsync(nameof(ClientHub.OnStockUpdated), _stockDb.Yellow, _stockDb.Cyan, _stockDb.Magenta);
            return message;
        }
    }
}
