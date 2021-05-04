using Microsoft.AspNetCore.Html;

namespace MfePoc.Reporting.Contract
{
    public static class ReportingControls
    {
        public static HtmlString Report()
        {
            return new HtmlString($"<iframe src=\"/Reporting/Control?control=SalesReport\" frameborder=\"0\" style=\"width:100%;\"></iframe>");
        }
    }
}
