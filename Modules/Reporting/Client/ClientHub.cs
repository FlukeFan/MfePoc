using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MfePoc.Sales.Contract;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace MfePoc.Reporting.Client
{
    public class ClientHub
    {
        public event Func<Task> OnReportUpdateAsync;

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
                hub.On(nameof(OnReportUpdated), _instance.OnReportUpdated);
                return _instance;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<string> RequestHostDetailAsync()
        {
            var response = await _hub.InvokeCoreAsync("RequestHostDetail", typeof(string), new object[0]);
            return (string)response;
        }

        public async Task<Sales> RequestSalesAsync()
        {
            var response = await _hub.InvokeCoreAsync("RequestSales", typeof(Sales), new object[0]);
            return (Sales)response;
        }

        public async Task OnReportUpdated()
        {
            await (OnReportUpdateAsync?.Invoke() ?? Task.CompletedTask);
        }

        private class KeepRetrying : IRetryPolicy
        {
            public TimeSpan? NextRetryDelay(RetryContext retryContext)
            {
                return TimeSpan.FromMilliseconds(500);
            }
        }

        public class Sales
        {
            public decimal Total { get; set; }
            public IList<OnSellExecuted> Items {get;set;}
        }
    }
}
