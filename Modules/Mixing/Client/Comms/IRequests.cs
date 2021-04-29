using System.Threading.Tasks;

namespace MfePoc.Mixing.Client.Comms
{
    public interface IRequests
    {
        string RequestHostDetail();
        StockLevelResponse RequestStockLevels();
        Task<string> RequestMixAsync(int red, int green, int blue);
    }
}
