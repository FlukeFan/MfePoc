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

        public async Task<string> MixAsync(int red, int green, int blue)
        {
            await Task.CompletedTask;

            if (red > Red)
                return "You need more Red";

            if (green > Green)
                return "You need more Green";

            if (blue > Blue)
                return "You need more Blue";

            if (blue == 0)
            {
                // mixing yellow
                Red -= red;
                Green -= green;
                Yellow += 1;
                return null;
            }

            if (red == 0)
            {
                // mixing cyan
                Green -= green;
                Blue -= blue;
                Cyan += 1;
                return null;
            }

            if (green == 0)
            {
                // mixing magenta
                Red -= red;
                Blue -= blue;
                Magenta += 1;
                return null;
            }

            return "Unidentified colour being mixed";
        }

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
                await _stockDb.OnUpdatedAsync();
            }
        }
    }
}
