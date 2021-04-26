using Microsoft.AspNetCore.Html;

namespace MfePoc.Generation.Contract
{
    public static class GenerationControls
    {
        public static HtmlString StockLevels()
        {
            return FramedControl("StockLevels", 156, 52);
        }

        public static HtmlString RedGenerator()
        {
            return FramedControl("RedButton", 150, 40);
        }

        public static HtmlString GreenGenerator()
        {
            return FramedControl("GreenButton", 150, 40);
        }

        public static HtmlString BlueGenerator()
        {
            return FramedControl("BlueButton", 150, 40);
        }

        private static HtmlString FramedControl(string name, int width, int height)
        {
            return new HtmlString($"<iframe src=\"/Generation/Control?control={name}\" frameborder=\"0\" style=\"width:{width}px; height:{height}px\"></iframe>");
        }
    }
}
