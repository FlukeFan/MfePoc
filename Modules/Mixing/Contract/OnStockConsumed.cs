namespace MfePoc.Mixing.Contract
{
    /// <summary> event to let this module know that stock was consumed </summary>
    public class OnStockConsumed
    {
        public int Yellow { get; set; }
        public int Cyan { get; set; }
        public int Magenta { get; set; }
    }
}
