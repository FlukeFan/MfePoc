using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MfePoc.Shared.Bus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ZipDeploy;

namespace MfePoc.Mixing.Server
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
            services.AddZipDeploy();

            services.AddDumbFileBus("Mixing", GetType().Assembly);
            services.AddHostedService<HostStartup>();
            services.AddSignalR();

            var mvcBuilder = services.AddRazorPages();

            if (HostEnvironment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();

                services.Configure<MvcRazorRuntimeCompilationOptions>(opt =>
                {
                    var libPath = Path.Combine(HostEnvironment.ContentRootPath, "..", "..", "..", "Shared");
                    var libFullPath = Path.GetFullPath(libPath);
                    opt.FileProviders.Add(new PhysicalFileProvider(libFullPath));
                });
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MixingHub>("/hub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        private class HostStartup : IHostedService
        {
            private IBus _bus;

            public HostStartup(IBus bus)
            {
                _bus = bus;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                await _bus.PublishAsync(new Generation.Contract.OnServiceStarted { Name = "Mixing" });
            }

            public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        }
    }
}
