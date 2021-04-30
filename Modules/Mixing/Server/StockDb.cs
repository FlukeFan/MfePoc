using System.Threading.Tasks;
using MfePoc.Generation.Contract;
using MfePoc.Mixing.Client.Comms;
using MfePoc.Shared.Bus;

namespace MfePoc.Mixing.Server
{
    public class StockDb
    {
        private readonly IBus _bus;

        public StockDb(IBus bus)
        {
            _bus = bus;
        }

        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }

        public int Yellow { get; private set; }
        public int Cyan { get; private set; }
        public int Magenta { get; private set; }

        public async Task<string> MixAsync(int red, int green, int blue)
        {
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
                await RaiseConsumed(red, green, 0);
                return null;
            }

            if (red == 0)
            {
                // mixing cyan
                Green -= green;
                Blue -= blue;
                Cyan += 1;
                await RaiseConsumed(0, green, blue);
                return null;
            }

            if (green == 0)
            {
                // mixing magenta
                Red -= red;
                Blue -= blue;
                Magenta += 1;
                await RaiseConsumed(red, 0, blue);
                return null;
            }

            return "Unidentified colour being mixed";
        }

        private async Task RaiseConsumed(int red, int green, int blue)
        {
            await _bus.PublishAsync(new OnStockConsumed
            {
                Red = red,
                Green = green,
                Blue = blue,
            });
        }

        public StockLevelResponse Levels()
        {
            return new StockLevelResponse
            {
                Yellow = Yellow,
                Cyan = Cyan,
                Magenta = Magenta,
            };
        }

        private class PrimaryStockUpdated : IHandle<Generation.Contract.OnStockUpdated>
        {
            private readonly StockDb _stockDb;

            public PrimaryStockUpdated(StockDb stockDb)
            {
                _stockDb = stockDb;
            }

            public Task HandleAsync(Generation.Contract.OnStockUpdated message)
            {
                _stockDb.Red = message.Red;
                _stockDb.Green = message.Green;
                _stockDb.Blue = message.Blue;
                return Task.CompletedTask;
            }
        }
    }
}
