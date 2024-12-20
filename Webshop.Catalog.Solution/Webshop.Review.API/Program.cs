using FluentAssertions.Common;
using MediatR;
using Prometheus;
using Serilog;
using System.Reflection;
using Webshop.Application;
using Webshop.Application.Contracts;
using Webshop.Data.Persistence;
using Webshop.Review.API.Utilities;
using Webshop.Review.Application;
using Webshop.Review.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom services
builder.Services.AddScoped<DataContext, DataContext>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));
// Add the instance singleton
builder.Services.AddSingleton<InstanceHelper>();
// Add health checks
builder.Services.AddHealthChecks();

// Use Serilog
var configuration = builder.Configuration;
string seqUrl = configuration.GetValue<string>("Settings:SeqLogAddress");
Console.WriteLine("Starting up");
Console.WriteLine($"SeqUrl: {seqUrl}");

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Service", "Review.Api") // Enrich with the tag "Service" and the name of this service
    .WriteTo.Seq(seqUrl)
    .CreateLogger();

// Add Serilog
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddReviewApplicationServices();
builder.Services.AddReviewInfrastructureServices();

// Add RabbitMQ Consumer Service
builder.Services.AddHostedService<RabbitMqConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Always show Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

// Enable Prometheus metrics
app.UseHttpMetrics();
app.MapControllers();
app.MapHealthChecks("/health");
app.MapMetrics();
app.MapGet("/", () => "Microservice: Review API Service");
app.Run();
