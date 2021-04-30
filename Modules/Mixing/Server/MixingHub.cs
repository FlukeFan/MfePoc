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
            return _stockDb.Levels();
        }

        public async Task<string> RequestMixAsync(int red, int green, int blue)
        {
            var message = await _stockDb.MixAsync(red, green, blue);
            await Clients.All.SendAsync(nameof(ClientHub.OnStockUpdated), _stockDb.Levels());
            return message;
        }
    }
}
