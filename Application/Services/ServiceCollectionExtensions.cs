using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IChatGroupService, ChatGroupService>();

            return services;
        }
    }
}
