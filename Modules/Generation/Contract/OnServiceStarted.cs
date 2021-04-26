namespace MfePoc.Generation.Contract
{
    /// <summary> event raised by modules to indicate they might want to know the current stock levels </summary>
    public class OnServiceStarted
    {
        public string Name { get; set; }
    }
}
