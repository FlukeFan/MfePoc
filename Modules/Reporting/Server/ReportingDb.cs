using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MfePoc.Reporting.Client;
using MfePoc.Sales.Contract;
using MfePoc.Shared.Bus;

namespace MfePoc.Reporting.Server
{
    public class ReportingDb
    {
        private IList<OnSellExecuted> _sales = new List<OnSellExecuted>();

        public ClientHub.Sales GetSales()
        {
            return new ClientHub.Sales
            {
                Total = decimal.Round(_sales.Sum(s => s.Amount), 2),
                Items = _sales.OrderBy(s => s.WhenUtc).ToList(),
            };
        }

        private class SellExecuted : IHandle<OnSellExecuted>
        {
            private readonly ReportingDb _reportingDb;

            public SellExecuted(ReportingDb reportingDb)
            {
                _reportingDb = reportingDb;
            }

            public Task HandleAsync(OnSellExecuted message)
            {
                _reportingDb._sales.Add(message);
                return Task.CompletedTask;
            }
        }
    }
}
