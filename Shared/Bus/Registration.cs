using Microsoft.Extensions.DependencyInjection;

namespace MfePoc.Shared.Bus
{
    public static class Registration
    {
        public static void AddDumbFileBus(this IServiceCollection services, string name)
        {
            services.AddSingleton<IBus, DumbFileBus>();
            services.AddSingleton(sp => (IBusControl)sp.GetRequiredService<IBus>());

            services.AddSingleton(new Host.BusName { Name = name });
            services.AddHostedService<Host>();
        }
    }
}
