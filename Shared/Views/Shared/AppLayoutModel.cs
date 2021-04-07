using Microsoft.AspNetCore.Mvc.Razor;

namespace MfePoc.Shared.Views.Shared
{
    public class AppLayoutModel
    {
        public string Title;
        public string BaseUrl;
        public bool EnableBlazorServerSide;
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
            bool enableBlazorServerSide = false)
        {
            page.Layout = "_appLayout";

            var model = new AppLayoutModel
            {
                Title = title,
                BaseUrl = baseUrl,
                EnableBlazorServerSide = enableBlazorServerSide,
            };

            page.ViewContext.ViewData["appLayoutModel"] = model;
        }
    }
}
