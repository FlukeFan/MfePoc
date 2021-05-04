using System;
using System.Threading;
using System.Threading.Tasks;
using MfePoc.Shared.Bus;

namespace MfePoc.Sales
{
    public class StockDb
    {
        public class CurrentWorth
        {
            public decimal ExchangeRate;

            public decimal UnitWhitePackageWorth;
            public decimal UnitColourPackageWorth;

            public int WhitePackageCount;
            public decimal WhitePackageWorth;

            public int YellowPackageCount;
            public decimal YellowPackageWorth;

            public int CyanPackageCount;
            public decimal CyanPackageWorth;

            public int MagentaPackageCount;
            public decimal MagentaPackageWorth;

            public decimal TotalWorth;
        }

        private readonly IBus _bus;
        private readonly Timer _timer;

        private decimal _exchangeRate;

        public event Func<Task> OnStockUpdateAsync;

        public StockDb(IBus bus)
        {
            _bus = bus;
            _exchangeRate = 1.00m;

            _timer = new Timer(s => UpdateExchangeRate().GetAwaiter().GetResult());
            _timer.Change(TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(4));
        }

        public int Yellow { get; private set; }
        public int Cyan { get; private set; }
        public int Magenta { get; private set; }

        public CurrentWorth CalculateCurrentWorth()
        {
            var yellow = Yellow;
            var cyan = Cyan;
            var magenta = Magenta;

            var white = Math.Min(yellow, Math.Min(cyan, magenta));
            yellow -= white;
            cyan -= white;
            magenta -= white;

            var worth = new CurrentWorth
            {
                ExchangeRate = decimal.Round(_exchangeRate, 2),

                WhitePackageCount = white,

                YellowPackageCount = yellow,

                CyanPackageCount = cyan,

                MagentaPackageCount = magenta,
            };

            return worth;
        }

        private async Task UpdateExchangeRate()
        {
            _exchangeRate = 0.5m + (new Random().Next(0, 100) / 100m);
            await OnUpdatedAsync();
        }

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
