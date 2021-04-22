using System.Threading.Tasks;
using MfePoc.Generation.Contract;
using MfePoc.Shared.Bus;

namespace MfePoc.Generation
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

        private async Task OnUpdatedAsync()
        {
            await _bus.PublishAsync(new OnStockUpdated
            {
                Red = Red,
                Green = Green,
                Blue = Blue,
            });
        }
    }
}
