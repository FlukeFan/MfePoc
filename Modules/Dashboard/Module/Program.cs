using MfePoc.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MfePoc.Dashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logging.SetupNLog<Program>(() =>
                CreateHostBuilder(args).Build().Run());
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseMfePocNLog();
                });
    }
}
