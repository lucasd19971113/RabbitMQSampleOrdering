using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQOrdering.Queue.Api.Extensions;
using RabbitMQOrdering.Queue.Api.Extensions.DIExtensions;
using RabbitMQOrdering.Queue.Api.Helpers;

namespace RabbitMQOrdering.Queue.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder ()
                .SetBasePath (env.ContentRootPath)
                .AddJsonFile ("appsettings.json", optional : true, reloadOnChange : true)
                .AddJsonFile ($"appsettings.{env.EnvironmentName}.json", optional : true)
                .AddEnvironmentVariables ();
            Configuration = builder.Build ();
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ServiceProvider { get; set; } 

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            

            var rabbitMQConfigurations = new RabbitMQConfigurations();
            
            new ConfigureFromConfigurationOptions<RabbitMQConfigurations>(
                Configuration.GetSection("RabbitMQConfigurations"))
                    .Configure(rabbitMQConfigurations);


            var factory = new ConnectionFactory () {
                HostName = rabbitMQConfigurations.HostName,
                Port = rabbitMQConfigurations.Port,
                UserName = rabbitMQConfigurations.UserName,
                Password = rabbitMQConfigurations.Password,
                VirtualHost = rabbitMQConfigurations.VirtualHost
            };

            

            services.AddSingleton(factory);


            

            var connection = factory.CreateConnection ();
            RabbitMQExtensions.EnsureExistRabbitMqVirtualHost(rabbitMQConfigurations, connection);

            var channel = connection.CreateModel () ;


            services.AddScopedServices();
            services.AddTransientServices();
            services.AddSingletonServices(ServiceProvider, Configuration);

            channel.AddChannels();
            //channel.ConfirmSelect();

            services.AddSingleton(rabbitMQConfigurations);
            services.AddLogging();
            services.AddSingleton(channel);
            services.AddSingleton(connection);

           


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
