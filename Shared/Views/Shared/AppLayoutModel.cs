using Microsoft.AspNetCore.Mvc.Razor;

namespace MfePoc.Shared.Views.Shared
{
    internal class AppLayoutModel
    {
        public string Title;
        public string BaseUrl;
    }

    public static class AppLayoutExtensions
    {
        public static void SetAppLayout(
            this IRazorPage page,
            string title,
            string baseUrl = null)
        {
            page.Layout = "_appLayout";

            var model = new AppLayoutModel
            {
                Title = title,
                BaseUrl = baseUrl,
            };

            page.ViewContext.ViewData["appLayoutModel"] = model;

            // until all ViewData references are converted ...
            page.ViewContext.ViewData["Title"] = model.Title;
            page.ViewContext.ViewData["BaseUrl"] = model.BaseUrl;
        }
    }
}
