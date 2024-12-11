using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Webshop.Core.Contracts.Contracts;
using Webshop.Search.Application.Features.SearchProduct.Dtos;
using Webshop.Search.Application.Features.SearchProduct.Queries.GetAllProducts;
using Webshop.Search.Application.Features.SearchProduct.Queries.GetProductById;
using Webshop.Search.Application.Features.SearchProduct.Queries.GetProductsByCategory;
using Webshop.Search.Application.Features.SearchProduct.Queries.SearchProducts;
using System;

namespace Webshop.Search.Api.Controllers
{
    [Route("api/search/products")]
    [ApiController]
    public class ProductSearchController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductSearchController> _logger;
        private readonly IMapper _mapper;
        private readonly IRedisConnection _redisConnection;

        public ProductSearchController(
            IMediator mediator,
            ILogger<ProductSearchController> logger,
            IMapper mapper,
            IRedisConnection redisConnection)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
            _redisConnection = redisConnection;
        }

        /// <summary>
        /// Søger efter alle produkter med caching.
        /// </summary>
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var cache = _redisConnection.GetDatabase();
            const string cacheKey = "allProducts";

            // Tjek cache
            var cachedData = await cache.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("Returning cached products.");
                var cachedResult = JsonSerializer.Deserialize<IEnumerable<SearchProductDto>>(cachedData);
                return Ok(cachedResult);
            }

            var query = new GetAllProductsQuery();
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                _logger.LogInformation("No products found.");
                return NotFound("No products available.");
            }

            // Cache resultater
            await cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(result), TimeSpan.FromMinutes(10));
            return Ok(result);
        }

        /// <summary>
        /// Henter et produkt baseret på dets ID med caching.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var cache = _redisConnection.GetDatabase();
            var cacheKey = $"product:{id}";

            // Tjek cache
            var cachedData = await cache.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation($"Returning cached product for ID {id}.");
                var cachedResult = JsonSerializer.Deserialize<SearchProductDto>(cachedData);
                return Ok(cachedResult);
            }

            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogWarning("Product with ID {id} not found.", id);
                return NotFound($"Product with ID {id} not found.");
            }

            // Cache resultatet
            await cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(result), TimeSpan.FromMinutes(10));
            return Ok(result);
        }

        /// <summary>
        /// Henter produkter baseret på en kategori med caching.
        /// </summary>
        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetProductsByCategory(int id)
        {
            var cache = _redisConnection.GetDatabase();
            var cacheKey = $"products:category:{id}";

            // Tjek cache
            var cachedData = await cache.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation($"Returning cached products for category ID {id}.");
                var cachedResult = JsonSerializer.Deserialize<IEnumerable<SearchProductDto>>(cachedData);
                return Ok(cachedResult);
            }

            var query = new GetProductsByCategoryQuery(id);
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                _logger.LogInformation("No products found for category ID {id}.", id);
                return NotFound($"No products found for category ID {id}.");
            }

            // Cache resultater
            await cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(result), TimeSpan.FromMinutes(10));
            return Ok(result);
        }

        /// <summary>
        /// Søger efter produkter baseret på forskellige kriterier.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string name = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            if (name == null && categoryId == null && minPrice == null && maxPrice == null)
            {
                _logger.LogInformation("No specific search parameters provided, defaulting to price-based search.");
                minPrice = 0;
                maxPrice = decimal.MaxValue;
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
