using System;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQOrdering.Api.Commands;
using RabbitMQOrdering.Api.Context;
using RabbitMQOrdering.Api.Infrastructure.Data.Background;
using RabbitMQOrdering.Api.Infrastructure.RabbitMQ;
using RabbitMQOrdering.Api.Repository;
using RabbitMQOrdering.Api.Repository.IRepository;
using RabbitMQOrdering.Api.Services;
using RabbitMQOrdering.Api.Services.IServices;

namespace RabbitMQOrdering.Api.Extensions.DIExtensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IEntityFrameworkExtensions, EntityFrameworkExtensions>();

            services.AddScoped<IRabbitMQConsumeHelper, RabbitMQConsumeHelper>();
            
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IProductOrderService, ProductOrderService>();
            services.AddScoped<IProductOrderRepository, ProductOrderRepository>();

            services.AddScoped<IKitchenAreaService, KitchenAreaService>();
            services.AddScoped<IKitchenAreaRepository, KitchenAreaRepository>();

            return services;
        }

        public static IServiceCollection AddTransientServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddSingletonServices(this IServiceCollection services, IServiceProvider serviceProvider)
        {

            services.AddDbContext<AppDbContext> ();

            services.AddSingleton<IWorkQueue, WorkQueue>();

            services.AddSingleton<CommandHandler>();

            services.AddHostedService<ConsumeRabbitMQHostedService>();

            services.AddSingleton<IServiceProvider>(sp => sp);
            
            services.AddHostedService<DataBaseHostedService>();

            services.AddSingleton<ProductQueueService>();

            return services;
        }
    }
}