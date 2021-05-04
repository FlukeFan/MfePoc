using System;
using System.Threading;
using System.Threading.Tasks;
using MfePoc.Mixing.Contract;
using MfePoc.Sales.Contract;
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
            var exchangeRate = _exchangeRate;
            var yellow = Yellow;
            var cyan = Cyan;
            var magenta = Magenta;

            var white = Math.Min(yellow, Math.Min(cyan, magenta));
            yellow -= white;
            cyan -= white;
            magenta -= white;

            var unitWhitePackageWorth = 16m * exchangeRate;
            var unitColourPackageWorth = 4m * exchangeRate;

            var whitePackageWorth = white * unitWhitePackageWorth;
            var yellowPackageWorth = yellow * unitColourPackageWorth;
            var cyanPackageWorth = cyan * unitColourPackageWorth;
            var magentaPackageWorth = magenta * unitColourPackageWorth;
            var totalWorth = whitePackageWorth + yellowPackageWorth + cyanPackageWorth + magentaPackageWorth;

            return new CurrentWorth
            {
                ExchangeRate = decimal.Round(_exchangeRate, 2),

                UnitWhitePackageWorth = decimal.Round(unitWhitePackageWorth, 2),
                UnitColourPackageWorth = decimal.Round(unitColourPackageWorth, 2),

                WhitePackageCount = white,
                WhitePackageWorth = decimal.Round(whitePackageWorth, 2),

                YellowPackageCount = yellow,
                YellowPackageWorth = decimal.Round(yellowPackageWorth, 2),

                CyanPackageCount = cyan,
                CyanPackageWorth = decimal.Round(cyanPackageWorth, 2),

                MagentaPackageCount = magenta,
                MagentaPackageWorth = decimal.Round(magentaPackageWorth, 2),

                TotalWorth = decimal.Round(totalWorth, 2),
            };
        }

        public async Task SellAsync(CurrentWorth worth)
        {
            var consumedYellow = worth.WhitePackageCount + worth.YellowPackageCount;
            var consumedCyan = worth.WhitePackageCount + worth.CyanPackageCount;
            var consumedMagenta = worth.WhitePackageCount + worth.MagentaPackageCount;

            Yellow -= consumedYellow;
            Cyan -= consumedCyan;
            Magenta -= consumedMagenta;

            await _bus.PublishAsync(new OnStockConsumed
            {
                Yellow = consumedYellow,
                Cyan = consumedCyan,
                Magenta = consumedMagenta,
            });

            await _bus.PublishAsync(new OnSellExecuted
            {
                WhenUtc = DateTime.UtcNow,
                Description = $"{worth.WhitePackageCount} White, {worth.YellowPackageCount} Yellow, {worth.CyanPackageCount} Cyan, and {worth.MagentaPackageCount} Magenta",
                Amount = worth.TotalWorth,
            });
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
