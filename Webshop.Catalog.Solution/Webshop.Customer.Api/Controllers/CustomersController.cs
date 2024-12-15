using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Customer.Application.Features.CreateCustomer;
using Webshop.Customer.Application.Features.DeleteCustomer;
using Webshop.Customer.Application.Features.Dto;
using Webshop.Customer.Application.Features.GetCustomer;
using Webshop.Customer.Application.Features.GetCustomers;
using Webshop.Customer.Application.Features.Requests;
using Webshop.Customer.Application.Features.UpdateCustomer;
using Webshop.Domain.Common;
using Webshop.Messaging;

namespace Webshop.Customer.Api.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : BaseController
    {
        private IDispatcher dispatcher;
        private IMapper mapper;
        private ILogger<CustomersController> logger;
        private readonly RbqCustomerProducer _producer;  // Tilføj RabbitMQ-producenten
        public CustomersController(IDispatcher dispatcher, IMapper mapper, ILogger<CustomersController> logger, RbqCustomerProducer producer)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.dispatcher = dispatcher;
            _producer = producer;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            GetCustomersQuery query = new GetCustomersQuery();
            Result<List<CustomerDto>> result = await this.dispatcher.Dispatch(query);
            if (result.Success)
            {
                return FromResult<List<CustomerDto>>(result);
            }
            else
            {
                this.logger.LogError(result.Error.Message);
                return Error(result.Error);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            GetCustomerQuery query = new GetCustomerQuery(id);
            Result<CustomerDto> result = await this.dispatcher.Dispatch(query);
            if(result.Success)
            {
                return FromResult<CustomerDto>(result);
            }
            else
            {
                this.logger.LogError(result.Error.Message);
                return Error(result.Error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            CreateCustomerRequest.Validator validator = new CreateCustomerRequest.Validator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                this.logger.LogError(string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)));
                return Error(validationResult.Errors);
            }

            // Opret kunden
            var customer = this.mapper.Map<Domain.AggregateRoots.Customer>(request);
            var createCommand = new CreateCustomerCommand(customer);
            Result createResult = await this.dispatcher.Dispatch(createCommand);

            if (!createResult.Success)
            {
                this.logger.LogError(createResult.Error.Message);
                return Error(createResult.Error);
            }

            // Serialiser DTO til JSON
            var customerDto = this.mapper.Map<CustomerDto>(customer);
            var jsonMessage = System.Text.Json.JsonSerializer.Serialize(customerDto);

            // Send RabbitMQ-besked
            await _producer.SendMessageAsync(jsonMessage); // Nu sender du en string
            this.logger.LogInformation($"Customer '{customer.Name}' created and message sent to RabbitMQ.");

            return Ok($"Customer '{customer.Name}' successfully created and message sent.");
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            DeleteCustomerCommand command = new DeleteCustomerCommand(id);
            Result result = await this.dispatcher.Dispatch(command);
            if (result.Success)
            {
                return FromResult(result);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Error.Message));
                return Error(result.Error);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromBody]UpdateCustomerRequest request)
        {
            UpdateCustomerRequest.Validator validator = new UpdateCustomerRequest.Validator();
            var result = validator.Validate(request);
            if (result.IsValid)
            {
                Domain.AggregateRoots.Customer customer = this.mapper.Map<Domain.AggregateRoots.Customer>(request);
                UpdateCustomerCommand command = new UpdateCustomerCommand(customer);
                Result createResult = await this.dispatcher.Dispatch(command);
                return Ok(createResult);
            }
            else
            {
                this.logger.LogError(string.Join(",", result.Errors.Select(x => x.ErrorMessage)));
                return Error(result.Errors);
            }
        }
    }
}
