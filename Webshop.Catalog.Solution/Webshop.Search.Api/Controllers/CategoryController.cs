using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Webshop.Core.Contracts.Contracts;
using Webshop.Search.Application.Features.SearchCategory.Dtos;
using Webshop.Search.Application.Features.SearchCategory.Queries.GetAllCategories;
using Webshop.Search.Application.Features.SearchCategory.Queries.GetCategoryById;
using Webshop.Search.Application.Features.SearchCategory.Queries.GetChildCategories;
using Webshop.Search.Application.Features.SearchCategory.Queries.ExistsCategory;
using MediatR;
using System;

namespace Webshop.Search.Api.Controllers
{
    [Route("api/search/categories")]
    [ApiController]
    public class SearchController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SearchController> _logger;
        private readonly IRedisConnection _redisConnection;

        public SearchController(IMediator mediator, ILogger<SearchController> logger, IRedisConnection redisConnection)
        {
            _mediator = mediator;
            _logger = logger;
            _redisConnection = redisConnection;
        }

        /// <summary>
        /// Henter alle kategorier med caching.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var cache = _redisConnection.GetDatabase();
            const string cacheKey = "allCategories";

            // Check cache
            var cachedData = await cache.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("Returning cached categories.");
                var cachedResult = JsonSerializer.Deserialize<IEnumerable<SearchCategoryDto>>(cachedData);
                return Ok(cachedResult);
            }

            // Fetch data and cache it
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                _logger.LogWarning("No categories found.");
                return NoContent();
            }

            await cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(result), TimeSpan.FromMinutes(10));
            return Ok(result);
        }

        /// <summary>
        /// Henter en kategori baseret på dens ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var cache = _redisConnection.GetDatabase();
            var cacheKey = $"category:{id}";

            // Check cache
            var cachedData = await cache.StringGetAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation($"Returning cached category for ID {id}.");
                var cachedResult = JsonSerializer.Deserialize<SearchCategoryDto>(cachedData);
                return Ok(cachedResult);
            }

            // Fetch data and cache it
            var query = new GetCategoryByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found.", id);
                return NotFound($"Category with ID {id} not found.");
            }

            await cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(result), TimeSpan.FromMinutes(10));
            return Ok(result);
        }

        /// <summary>
        /// Henter underkategorier for en given kategori.
        /// </summary>
        [HttpGet("{id}/categories")]
        public async Task<IActionResult> GetChildCategories(int id)
        {
            var query = new GetChildCategoriesQuery(id);

            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                _logger.LogWarning("No child categories found for category ID {CategoryId}.", id);
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Tjekker, om en kategori eksisterer.
        /// </summary>
        [HttpGet("{parentId}/exists")]
        public async Task<IActionResult> ExistsCategory(int parentId)
        {
            var query = new ExistsSearchCategoryQuery(parentId);
            var exists = await _mediator.Send(query);

            if (!exists)
            {
                _logger.LogWarning("Category with ParentId {ParentId} does not exist.", parentId);
                return NotFound($"Category with ParentId {parentId} does not exist.");
            }

            return Ok(new { Exists = true });
        }
    }
}
