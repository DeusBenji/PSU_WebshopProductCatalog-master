using Microsoft.AspNetCore.Mvc;

namespace WebShop.Search.Api.Controllers
{
    
        public class SearchController : ControllerBase
        {
            private readonly ISearchService _searchService;

            public SearchController(ISearchService searchService)
            {
                _searchService = searchService;
            }

            [HttpGet("products")]
            public async Task<IActionResult> SearchProducts(string query)
            {
                var products = await _searchService.SearchProductsAsync(query);
                if (!products.Any())
                    return NotFound("No products match your search.");
                return Ok(products);
            }
        }

    
}
