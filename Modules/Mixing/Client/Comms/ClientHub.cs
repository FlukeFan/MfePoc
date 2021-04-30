using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace MfePoc.Mixing.Client.Comms
{
    public class ClientHub
    {
        public event Action<StockLevelResponse> OnStockUpdate;

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
                    .WithAutomaticReconnect(new KeepRetrying())
                    .Build();

                await hub.StartAsync();

                _instance = new ClientHub { _hub = hub };
                hub.On<StockLevelResponse>(nameof(OnStockUpdated), _instance.OnStockUpdated);
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

        public async Task<StockLevelResponse> RequestStockLevelsAsync()
        {
            var response = await _hub.InvokeCoreAsync(nameof(IRequests.RequestStockLevels), typeof(StockLevelResponse), new object[0]);
            return (StockLevelResponse)response;
        }

        public async Task<string> RequestMixAsync(int red, int green, int blue)
        {
            var response = await _hub.InvokeCoreAsync(nameof(IRequests.RequestMixAsync), typeof(string), new object[] { red, green, blue });
            return (string)response;
        }

        public void OnStockUpdated(StockLevelResponse levels)
        {
            OnStockUpdate?.Invoke(levels);
        }

        private class KeepRetrying : IRetryPolicy
        {
            public TimeSpan? NextRetryDelay(RetryContext retryContext)
            {
                return TimeSpan.FromMilliseconds(500);
            }
        }
    }
}
