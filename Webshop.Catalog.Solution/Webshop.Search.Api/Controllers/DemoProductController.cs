using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Api.Controllers
{
    [Route("api/demoproduct")]
    [ApiController]
    public class DemoProductController : BaseController
    {
        /// <summary>
        /// Henter et demo-produkt baseret på dets ID.
        /// </summary>
        /// <param name="id">Produktets ID.</param>
        /// <returns>Demo-produktdata.</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            // Simulerer et demo-produkt.
            var productDto = new SearchProductDto
            {
                Id = id,
                Name = "Demo Product",
                Description = "This is a demo product for testing purposes.",
                SKU = "DEMO-SKU",
                AmountInStock = 50,
                Price = 10000,
                Currency = "DKK",
                MinStock = 5
            };

            // Returnerer demo-produktet som JSON.
            return Ok(productDto);
        }
    }
}
