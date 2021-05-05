namespace MfePoc.Shared
{
    public static class FormattingExtensions
    {
        public static string FormatGBP(this decimal value)
        {
            return $"£{value:0.00}".Replace(",", ".");
        }
    }
}
