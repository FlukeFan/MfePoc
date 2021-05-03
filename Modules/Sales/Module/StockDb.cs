using System;
using System.Threading.Tasks;
using MfePoc.Shared.Bus;

namespace MfePoc.Sales
{
    public class StockDb
    {
        private readonly IBus _bus;

        public event Func<Task> OnStockUpdateAsync;

        public StockDb(IBus bus)
        {
            _bus = bus;
        }

        public int Yellow { get; private set; }
        public int Cyan { get; private set; }
        public int Magenta { get; private set; }

        private async Task OnUpdatedAsync()
        {
            await (OnStockUpdateAsync?.Invoke() ?? Task.CompletedTask);
        }

        private class SecondaryStockUpdated : IHandle<Mixing.Contract.OnStockUpdated>
        {
            private readonly StockDb _stockDb;

            public SecondaryStockUpdated(StockDb stockDb)
            {
                _stockDb = stockDb;
            }

            public async Task HandleAsync(Mixing.Contract.OnStockUpdated message)
            {
                _stockDb.Yellow = message.Yellow;
                _stockDb.Cyan = message.Cyan;
                _stockDb.Magenta = message.Magenta;
                await _stockDb.OnUpdatedAsync();
            }
        }
    }
}
