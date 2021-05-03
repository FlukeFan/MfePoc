namespace MfePoc.Mixing.Contract
{
    /// <summary> event to let other modules know the current state of the secondary colour stock </summary>
    public class OnStockUpdated
    {
        public int Yellow { get; set; }
        public int Cyan { get; set; }
        public int Magenta { get; set; }
    }
}
