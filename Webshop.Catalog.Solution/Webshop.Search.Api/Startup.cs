using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;
using System.Reflection;
using Webshop.Application;
using Webshop.Application.Contracts;
using Webshop.Search.Application;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Persistence;
using Webshop.Data.Persistence;

namespace Webshop.Search.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            string sequrl = Configuration.GetValue<string>("Settings:SeqLogAddress");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", "Search.API") // Enrich med navnet på denne service
                .WriteTo.Seq(sequrl)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // Tilføj tjenester til containeren
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Webshop.Search.Api", Version = "v1" });
            });

            // Tilføj repositories og datakontekst
            services.AddScoped<ISearchCategoryRepository, SearchCategoryRepository>();
            services.AddScoped<ISearchProductRepository, SearchProductRepository>();
            services.AddScoped<DataContext, DataContext>();

            // Tilføj MediatR og dispatcher
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));

            // Tilføj Search.Application-tjenester
            services.AddSearchApplicationServices();

            // Tilføj health checks
            services.AddHealthChecks();
        }

        // Konfigurer HTTP-request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Webshop.Search.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Konfigurer Serilog
            loggerFactory.AddSerilog();

            // Aktiver Prometheus-metrics
            app.UseHttpMetrics();
            app.UseHealthChecks("/health");
            app.UseMetricServer();
        }
    }
}
