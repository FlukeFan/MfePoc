using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace MfePoc.Mixing.Client
{
    public class ClientHub
    {
        private static SemaphoreSlim _lock = new SemaphoreSlim(1);
        private static ClientHub _instance;

        private HubConnection _hub;

        public static async Task<ClientHub> GetStartedHubAsync(NavigationManager navigationManager)
        {
            await _lock.WaitAsync();

            try
            {
                if (_instance != null)
                    return _instance;

                var hub = new HubConnectionBuilder()
                    .WithUrl(navigationManager.ToAbsoluteUri("hub"))
                    .Build();

                await hub.StartAsync();

                _instance = new ClientHub { _hub = hub };
                return _instance;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<string> RequestHostDetailAsync()
        {
            var response = await _hub.InvokeCoreAsync(nameof(IRequests.RequestHostDetail), typeof(string), new object[0]);
            return (string)response;
        }

        public interface IRequests
        {
            string RequestHostDetail();
        }
    }
}
