using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Application.Features.SearchCategoryWithProduct.Queries;
using Webshop.Search.Application.Features.SearchCategoryWithProduct.Dtos;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchCategoryWithProduct.Queries
{
    public class SearchCategoryWithProductsQueryHandler : IQueryHandler<SearchCategoryWithProductsQuery, SearchCategoryWithProductsDto>
    {
        private readonly ISearchCategoryRepository _searchCategoryRepository;

        public SearchCategoryWithProductsQueryHandler(ISearchCategoryRepository searchCategoryRepository)
        {
            _searchCategoryRepository = searchCategoryRepository;
        }

        public async Task<Result<SearchCategoryWithProductsDto>> Handle(SearchCategoryWithProductsQuery query, CancellationToken cancellationToken)
        {
            var category = await _searchCategoryRepository.GetByIdWithProductsAsync(query.CategoryId);
            if (category == null)
            {
                return Result.Fail<SearchCategoryWithProductsDto>("Category not found.");
            }

            var dto = new SearchCategoryWithProductsDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentId = category.ParentId,
                Products = category.Products.Select(product => new SearchProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Currency = product.Currency
                }).ToList()
            };

            return Result.Ok(dto);
        }
    }
}
