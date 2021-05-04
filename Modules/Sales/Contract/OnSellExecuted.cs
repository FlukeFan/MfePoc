using System;

namespace MfePoc.Sales.Contract
{
    /// <summary> event to let other modules know how much product was sold </summary>
    public class OnSellExecuted
    {
        public DateTime WhenUtc { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
