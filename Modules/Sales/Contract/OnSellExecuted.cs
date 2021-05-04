namespace MfePoc.Sales.Contract
{
    /// <summary> event to let other modules know how much product was sold </summary>
    public class OnSellExecuted
    {
        public decimal Amount { get; set; }
    }
}
