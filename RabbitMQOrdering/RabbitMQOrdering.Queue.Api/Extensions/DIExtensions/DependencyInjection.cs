using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQOrdering.Queue.Api.Infrastructure.RabbitMQ;

namespace RabbitMQOrdering.Queue.Api.Extensions.DIExtensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {            

            services.AddScoped<IConsumeRabbitMQService, ConsumeRabbitMQService>();

            return services;
        }

        public static IServiceCollection AddTransientServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddSingletonServices(this IServiceCollection services, IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            services.AddSingleton<IConfiguration> (Configuration);
            
            return services;
        }
    }
}