using Microsoft.AspNetCore.Html;

namespace MfePoc.Mixing.Contract
{
    public static class MixingControls
    {
        public static HtmlString StockLevels()
        {
            return FramedControl("StockLevels", 156, 52);
        }

        public static HtmlString ColourGenerators()
        {
            return FramedControl("Mixers", 150, 120);
        }

        private static HtmlString FramedControl(string name, int width, int height)
        {
            return new HtmlString($"<iframe src=\"/Mixing/Control?control={name}\" frameborder=\"0\" style=\"width:{width}px; height:{height}px\"></iframe>");
        }
    }
}
