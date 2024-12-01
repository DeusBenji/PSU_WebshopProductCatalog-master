using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, IEnumerable<SearchProductDto>>
    {
        private readonly ISearchProductRepository _productRepository;

        public GetProductsByCategoryQueryHandler(ISearchProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<SearchProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.SearchProductsAsync(null, request.CategoryId, null, null);

            return products.Select(product => new SearchProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Price = product.Price,
                Currency = product.Currency

            });
        }
    }
}
