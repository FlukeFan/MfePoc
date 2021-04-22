using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace MfePoc.Shared.Bus
{
    internal class Host : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly BusName _busName;

        public Host(IBusControl busControl, BusName busName)
        {
            _busControl = busControl;
            _busName = busName;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(_busName.Name);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        internal class BusName
        {
            public string Name { get; set; }
        }
    }
}
