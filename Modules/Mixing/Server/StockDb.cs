using System;
using System.Threading.Tasks;
using MfePoc.Shared.Bus;

namespace MfePoc.Mixing.Server
{
    public class StockDb
    {
        public event Func<Task> OnStockUpdateAsync;

        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }

        public int Yellow { get; private set; }
        public int Cyan { get; private set; }
        public int Magenta { get; private set; }

        private async Task OnUpdatedAsync()
        {
            await (OnStockUpdateAsync?.Invoke() ?? Task.CompletedTask);
        }

        private class PrimaryStockUpdated : IHandle<Generation.Contract.OnStockUpdated>
        {
            private readonly StockDb _stockDb;

            public PrimaryStockUpdated(StockDb stockDb)
            {
                _stockDb = stockDb;
            }

            public async Task HandleAsync(Generation.Contract.OnStockUpdated message)
            {
                _stockDb.Red = message.Red;
                _stockDb.Green = message.Green;
                _stockDb.Blue = message.Blue;
                await _stockDb.OnStockUpdateAsync();
            }
        }
    }
}
