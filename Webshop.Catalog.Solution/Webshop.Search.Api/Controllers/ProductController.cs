using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Webshop.Search.Application.Features.SearchProduct.Dtos;
using Webshop.Search.Application.Features.SearchProduct.Queries.GetAllProducts;
using Webshop.Search.Application.Features.SearchProduct.Queries.GetProductById;
using Webshop.Search.Application.Features.SearchProduct.Queries.GetProductsByCategory;
using Webshop.Search.Application.Features.SearchProduct.Queries.SearchProducts;

namespace Webshop.Search.Api.Controllers
{
    [Route("api/search/products")]
    [ApiController]
    public class ProductSearchController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductSearchController> _logger;
        private readonly IMapper _mapper;

        public ProductSearchController(IMediator mediator, ILogger<ProductSearchController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Søger efter alle produkter.
        /// </summary>
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                _logger.LogInformation("No products found.");
                return NotFound("No products available.");
            }

            return Ok(result);
        }

        /// <summary>
        /// Henter et produkt baseret på dets ID.
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogWarning("Product with ID {id} not found.", id);
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(result);
        }

        /// <summary>
        /// Henter produkter baseret på en kategori.
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            var query = new GetProductsByCategoryQuery(id); // Brug konstruktøren
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogWarning("Product with ID {id} not found.", id);
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(result);
        }


        /// <summary>
        /// Søger efter produkter baseret på forskellige kriterier.
        /// </summary>
        [HttpGet]
        [Route("search")]

        public async Task<IActionResult> SearchProducts(
    [FromQuery] string name = null,
    [FromQuery] int? categoryId = null,
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null)
        {
            // Tjek om ingen parametre er angivet
            if (name == null && categoryId == null && minPrice == null && maxPrice == null)
            {
                _logger.LogInformation("No specific search parameters provided, defaulting to price-based search.");
                minPrice = 0; // Minimum pris som standard
                maxPrice = decimal.MaxValue; // Maksimum pris som standard
            }

            var query = new SearchProductsQuery(name, categoryId, minPrice, maxPrice);
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                _logger.LogInformation("No products matched the search criteria.");
                return NotFound("No products matched the search criteria.");
            }

            return Ok(result);
        }

    }
}

