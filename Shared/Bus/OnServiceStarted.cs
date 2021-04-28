namespace MfePoc.Shared.Bus
{
    /// <summary> event raised by modules to indicate they might want to know the current state of the application </summary>
    public class OnServiceStarted
    {
        public string Name { get; set; }
    }
}
