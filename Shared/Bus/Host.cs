using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace MfePoc.Shared.Bus
{
    internal class Host : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly IBus _bus;
        private readonly BusName _busName;

        public Host(IBusControl busControl, IBus bus, BusName busName)
        {
            _busControl = busControl;
            _bus = bus;
            _busName = busName;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(_busName.Name);
            await _bus.PublishAsync(new OnServiceStarted { Name = _busName.Name });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        internal class BusName
        {
            public string Name { get; set; }
        }
    }
}
