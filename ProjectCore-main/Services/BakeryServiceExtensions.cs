using Microsoft.Extensions.DependencyInjection;
using ProjectCore.Interfaces;
using ProjectCore.Services;

namespace ProjectCore
{
    public static class BakeryServiceExtensions
    {
        public static IServiceCollection AddBakeryService(this IServiceCollection services)
        {
            services.AddSingleton<IBakeryService, BakeryService>();
            return services;
        }
    }
}
