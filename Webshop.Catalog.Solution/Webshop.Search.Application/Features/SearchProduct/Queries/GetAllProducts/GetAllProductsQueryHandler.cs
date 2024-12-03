using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<SearchProductDto>>
    {
        private readonly ISearchProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(ISearchProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllProductsAsync();
            return _mapper.Map<IEnumerable<SearchProductDto>>(products);
        }
    }
}
