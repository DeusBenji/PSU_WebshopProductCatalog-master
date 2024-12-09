using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Search.Application.Features.SearchCategory.Dtos;
using Webshop.Search.Application.Features.SearchCategory.Queries.GetAllCategories;
using Webshop.Search.Application.Features.SearchCategory.Queries.GetCategoryById;
using Webshop.Search.Application.Features.SearchCategory.Queries.GetChildCategories;
using Webshop.Search.Application.Features.SearchCategory.Queries.ExistsCategory;
using MediatR;

namespace Webshop.Search.Api.Controllers
{
    [Route("api/search/categories")]
    [ApiController]
    public class SearchController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IMediator mediator, ILogger<SearchController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Henter alle kategorier.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);

            if (result == null || !result.Any())
            {
                _logger.LogWarning("No categories found.");
                return NoContent();
            }

            return Ok(result);
        }

        /// <summary>
        /// Henter en kategori baseret på dens ID.
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found.", id);
                return NotFound($"Category with ID {id} not found.");
            }

            return Ok(result);
        }

        /// <summary>
        /// Henter underkategorier for en given kategori.
        /// </summary>
        [HttpGet]
        [Route("{id}/categories")]
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

        [HttpGet]
        [Route("{parentId}/exists")]
        public async Task<IActionResult> ExistsCategory(int parentId)
        {
            var query = new ExistsSearchCategoryQuery (parentId);
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
