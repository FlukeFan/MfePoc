using Microsoft.AspNetCore.Html;

namespace MfePoc.Sales.Contract
{
    public static class SalesControls
    {
        public static HtmlString SellControls()
        {
            return new HtmlString($"<iframe src=\"/Sales/Control?control=SellControls\" frameborder=\"0\" style=\"width:100%;\"></iframe>");
        }
    }
}
