using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace MfePoc.BlazorSS2
{
    public class Startup
    {
        public Startup(IWebHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;
        }

        public IWebHostEnvironment HostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddServerSideBlazor();

            services.Configure<MvcRazorRuntimeCompilationOptions>(opt =>
            {
                var libPath = Path.Combine(HostEnvironment.ContentRootPath, "..", "Shared");
                var libFullPath = Path.GetFullPath(libPath);
                opt.FileProviders.Add(new PhysicalFileProvider(libFullPath));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UsePathBase("/BlazorSS2");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
