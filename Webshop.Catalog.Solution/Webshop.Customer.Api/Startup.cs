using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;
using System.Reflection;
using System.Threading.Tasks;
using Webshop.Application;
using Webshop.Application.Contracts;
using Webshop.Customer.Application;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Customer.Persistence;
using Webshop.Data.Persistence;
using Webshop.Messaging;
using Webshop.Messaging.Contracts;

namespace Webshop.Customer.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            // Konfigurer Serilog
            string seqUrl = _configuration.GetValue<string>("Settings:SeqLogAddress");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", "Customer.API")
                .WriteTo.Seq(seqUrl)
                .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC
            services.AddControllers();

            // Swagger setup
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Webshop.Customer.Api",
                    Version = "v1"
                });
            });

            // RabbitMQ-producer setup
            services.AddSingleton<IRbqCustomerProducer>(sp =>
            {
                var producer = new RbqCustomerProducer("localhost", "CustomerExchange", "ReviewQueue");
                Task.Run(() => producer.InitializeAsync()).Wait();
                return producer;
            });


            // Add egne services
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<DataContext, DataContext>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));

            services.AddCustomerApplicationServices();

            // Health checks
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webshop.Customer.Api v1"));

            // HTTPS redirect
            app.UseHttpsRedirection();

            // Routing setup
            app.UseRouting();

            // Authorization setup
            app.UseAuthorization();

            // Endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Serilog logging
            loggerFactory.AddSerilog();

            // Prometheus metrics
            app.UseHttpMetrics();
            app.UseHealthChecks("/health");
            app.UseMetricServer();
        }
    }
}
