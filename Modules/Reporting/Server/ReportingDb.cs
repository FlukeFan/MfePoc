using System.Collections.Generic;
using System.Linq;
using MfePoc.Reporting.Client;
using MfePoc.Sales.Contract;

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
                Items = _sales,
            };
        }
    }
}
