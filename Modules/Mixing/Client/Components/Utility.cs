using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;

namespace MfePoc.Mixing.Client.Components
{
    public static class Utility
    {
        public static MarkupString Raw(this HtmlString value)
        {
            return new MarkupString(value.Value);
        }
    }
}
