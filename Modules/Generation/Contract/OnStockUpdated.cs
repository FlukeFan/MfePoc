namespace MfePoc.Generation.Contract
{
    /// <summary> event to let other modules know the current state of the primary colour stock </summary>
    public class OnStockUpdated
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
}
