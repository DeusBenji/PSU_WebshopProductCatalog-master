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
using Webshop.Messaging.Contracts;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
{
    private readonly ILogger<CreateCustomerCommandHandler> _logger;
    private readonly ICustomerRepository _customerRepository;
    private readonly IRbqCustomerProducer _producer;

    public CreateCustomerCommandHandler(
        ILogger<CreateCustomerCommandHandler> logger,
        ICustomerRepository customerRepository,
        IRbqCustomerProducer producer)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _producer = producer;
    }

    public async Task<Result> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            await _customerRepository.CreateAsync(command.Customer);

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

            await _producer.SendMessageAsync(JsonSerializer.Serialize(customerDto));

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ex.Message);
            return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
        }
    }
}
