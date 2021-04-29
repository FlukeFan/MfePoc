using Microsoft.AspNetCore.Mvc.Razor;

namespace MfePoc.Shared.Views.Shared
{
    public class AppLayoutModel
    {
        public string Title;
        public string BaseUrl;
        public bool EnableBlazorServerSide;
        public bool EnableBlazorClientSide;
        public string ClientStyles;
        public string CustomScript;
    }

    public static class AppLayoutExtensions
    {
        public static AppLayoutModel LayoutModel(this IRazorPage page)
        {
            return (AppLayoutModel)page.ViewContext.ViewData["appLayoutModel"];
        }

        public static void SetAppLayout(
            this IRazorPage page,
            string title,
            string baseUrl,
            bool enableBlazorServerSide = false,
            bool enableBlazorClientSide = false,
            string clientStyles = null,
            string customScript = null)
        {
            page.Layout = "_appLayout";

            var model = new AppLayoutModel
            {
                Title = title,
                BaseUrl = baseUrl,
                EnableBlazorServerSide = enableBlazorServerSide,
                EnableBlazorClientSide = enableBlazorClientSide,
                ClientStyles = clientStyles,
                CustomScript = customScript,
            };

            page.ViewContext.ViewData["appLayoutModel"] = model;
        }
    }
}
