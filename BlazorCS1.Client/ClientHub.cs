using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace MfePoc.BlazorCS1.Client
{
    public class ClientHub
    {
        private static HubConnection _hub;

        public static async Task<HubConnection> GetStartedHubAsync(NavigationManager navigationManager)
        {
            if (_hub != null)
                return _hub;

            _hub = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri("BlazorCS1/cs1hub"))
                .Build();

            await _hub.StartAsync();
            return _hub;
        }

        public class RequestHostDetail { }
    }
}
