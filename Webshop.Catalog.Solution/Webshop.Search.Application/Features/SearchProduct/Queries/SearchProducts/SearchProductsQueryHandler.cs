using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.SearchProducts
{
    public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, IEnumerable<SearchProductDto>>
    {
        private readonly ISearchProductRepository _productRepository;

        public SearchProductsQueryHandler(ISearchProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<SearchProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            // Filtrér produkter baseret på de angivne parametre
            var products = await _productRepository.SearchProductsAsync(
                request.Name,
                request.CategoryId,
                request.MinPrice,
                request.MaxPrice);

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