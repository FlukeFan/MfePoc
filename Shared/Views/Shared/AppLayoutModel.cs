using Microsoft.AspNetCore.Mvc.Razor;

namespace MfePoc.Shared.Views.Shared
{
    public class AppLayoutModel
    {
        public string Title;
        public string BaseUrl;
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
            string baseUrl)
        {
            page.Layout = "_appLayout";

            var model = new AppLayoutModel
            {
                Title = title,
                BaseUrl = baseUrl,
            };

            page.ViewContext.ViewData["appLayoutModel"] = model;
        }
    }
}
