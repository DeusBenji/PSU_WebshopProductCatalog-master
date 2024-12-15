using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Customer.Application.Features.Dto;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Domain.AggregateRoots;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMqConsumerService(IServiceProvider serviceProvider, ILogger<RabbitMqConsumerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        string queueName = "ReviewQueue";

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var jsonMessage = Encoding.UTF8.GetString(body);

            try
            {
                var customerDto = JsonSerializer.Deserialize<CustomerDto>(jsonMessage);

                using var scope = _serviceProvider.CreateScope();
                var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

                var customer = new Customer
                {
                    Name = customerDto.Name,
                    Email = customerDto.Email,
                    Address = customerDto.Address,
                    Address2 = customerDto.Address2,
                    City = customerDto.City,
                    Region = customerDto.Region,
                    PostalCode = customerDto.PostalCode,
                    Country = customerDto.Country
                };

                await customerRepository.CreateAsync(customer);
                _logger.LogInformation($"[x] Kunde oprettet: {customer.Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[!] Fejl ved behandling af besked: {ex.Message}");
            }
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: true,
            consumer: consumer
        );

        _logger.LogInformation("[*] Venter på beskeder...");
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
