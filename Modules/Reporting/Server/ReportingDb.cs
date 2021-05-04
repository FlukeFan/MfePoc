using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MfePoc.Reporting.Client;
using MfePoc.Sales.Contract;
using MfePoc.Shared.Bus;
using Microsoft.AspNetCore.SignalR;

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
                Items = _sales.OrderByDescending(s => s.WhenUtc).ToList(),
            };
        }

        private class SellExecuted : IHandle<OnSellExecuted>
        {
            private readonly ReportingDb _reportingDb;
            private readonly IHubContext<ReportingHub> _hub;

            public SellExecuted(ReportingDb reportingDb, IHubContext<ReportingHub> hub)
            {
                _reportingDb = reportingDb;
                _hub = hub;
            }

            public async Task HandleAsync(OnSellExecuted message)
            {
                _reportingDb._sales.Add(message);
                await _hub.Clients.All.SendAsync("OnReportUpdated");
            }
        }
    }
}
