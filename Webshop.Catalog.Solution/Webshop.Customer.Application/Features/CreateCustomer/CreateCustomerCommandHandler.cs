using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System;
using Webshop.Application.Contracts;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Customer.Application.Features.CreateCustomer;
using Webshop.Customer.Application.Features.Dto;
using Webshop.Domain.Common;
using Webshop.Messaging;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
{
    private readonly ILogger<CreateCustomerCommandHandler> _logger;
    private readonly ICustomerRepository _customerRepository;
    private readonly RbqCustomerProducer _producer;

    public CreateCustomerCommandHandler(
        ILogger<CreateCustomerCommandHandler> logger,
        ICustomerRepository customerRepository,
        RbqCustomerProducer producer)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _producer = producer;
    }

    public async Task<Result> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Opret kunde i din database
            await _customerRepository.CreateAsync(command.Customer);

            // 2. Opret DTO og send besked til RabbitMQ
            var customerDto = new CustomerDto
            {
                Id = command.Customer.Id,
                Name = command.Customer.Name,
                Email = command.Customer.Email,
                Address = command.Customer.Address,
                Address2 = command.Customer.Address2,
                City = command.Customer.City,
                Region = command.Customer.Region,
                PostalCode = command.Customer.PostalCode,
                Country = command.Customer.Country
            };

            // 3. Send besked
            await _producer.SendMessageAsync(customerDto);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
        }
    }
}
