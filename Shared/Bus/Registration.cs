using System;
using System.Linq;
using System.Reflection;
using MfePoc.Shared.Views.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace MfePoc.Shared.Bus
{
    public static class Registration
    {
        public static void AddDumbFileBus(this IServiceCollection services, string name, Assembly handlerAssembly)
        {
            services.AddSingleton<IBus, DumbFileBus>();
            services.AddSingleton(sp => (IBusControl)sp.GetRequiredService<IBus>());

            services.AddSingleton(new Host.BusName { Name = name });
            services.AddHostedService<Host>();
            services.AddTransient<IHandle<OnServiceStarted>, AppLayoutModel.ServiceStarted>();

            foreach (var handlerType in handlerAssembly.GetTypes())
                RegisterHandler(services, handlerType);
        }

        private static void RegisterHandler(IServiceCollection services, Type handlerType)
        {
            var handlerInterfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandle<>))
                .ToList();

            handlerInterfaces.ForEach(i =>
                services.AddTransient(i, handlerType));
        }
    }
}
