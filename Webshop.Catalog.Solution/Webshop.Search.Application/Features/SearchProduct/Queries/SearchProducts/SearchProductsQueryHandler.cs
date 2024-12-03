using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.SearchProducts
{
    public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, IEnumerable<SearchProductDto>>
    {
        private readonly ISearchProductRepository _productRepository;
        private readonly IMapper _mapper;

        public SearchProductsQueryHandler(ISearchProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.SearchProductsAsync(
                request.Name,
                request.CategoryId,
                request.MinPrice,
                request.MaxPrice);

            return _mapper.Map<IEnumerable<SearchProductDto>>(products);
        }
    }
}
