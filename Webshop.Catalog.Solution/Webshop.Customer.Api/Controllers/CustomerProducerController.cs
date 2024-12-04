using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Webshop.Messaging;

namespace Webshop.Customer.Api.Controllers
{
    public class MessageDto
    {
        public string Message { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class CustomerProducerController : ControllerBase
    {
        private readonly RbqCustomerProducer _producer;

        public CustomerProducerController(RbqCustomerProducer producer)
        {
            _producer = producer;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            await _producer.SendMessageAsync(dto.Message);
            return Ok($"Message '{dto.Message}' sent.");
        }
    }
}
