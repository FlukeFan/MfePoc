namespace MfePoc.Generation.Contract
{
    /// <summary> event to let this module know that stock was consumed </summary>
    public class OnStockConsumed
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
}
