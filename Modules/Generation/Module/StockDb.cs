using System;
using System.Threading.Tasks;
using MfePoc.Generation.Contract;
using MfePoc.Shared.Bus;

namespace MfePoc.Generation
{
    public class StockDb
    {
        private readonly IBus _bus;

        public event Func<Task> OnStockUpdateAsync;

        public StockDb(IBus bus)
        {
            _bus = bus;
        }

        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }

        public async Task GenerateRedAsync()
        {
            Red++;
            await OnUpdatedAsync();
        }

        public async Task GenerateGreenAsync()
        {
            Green++;
            await OnUpdatedAsync();
        }

        public async Task GenerateBlueAsync()
        {
            Blue++;
            await OnUpdatedAsync();
        }

        public async Task ConsumeAsync(OnStockConsumed consumed)
        {
            Red -= consumed.Red;
            Green -= consumed.Green;
            Blue -= consumed.Blue;
            await OnUpdatedAsync();
        }

        private async Task OnUpdatedAsync()
        {
            await (OnStockUpdateAsync?.Invoke() ?? Task.CompletedTask);

            await _bus.PublishAsync(new OnStockUpdated
            {
                Red = Red,
                Green = Green,
                Blue = Blue,
            });
        }

        private class StockConsumed : IHandle<OnStockConsumed>
        {
            private readonly StockDb _stockDb;

            public StockConsumed(StockDb stockDb)
            {
                _stockDb = stockDb;
            }

            public async Task HandleAsync(OnStockConsumed message)
            {
                await _stockDb.ConsumeAsync(message);
            }
        }

        private class ServiceStarted : IHandle<OnServiceStarted>
        {
            private readonly StockDb _stockDb;

            public ServiceStarted(StockDb stockDb)
            {
                _stockDb = stockDb;
            }

            public async Task HandleAsync(OnServiceStarted message)
            {
                await _stockDb.OnUpdatedAsync();
            }
        }
    }
}
