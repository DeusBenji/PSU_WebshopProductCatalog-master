using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, SearchProductDto>
    {
        private readonly ISearchProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(ISearchProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<SearchProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.Id);
            return product == null ? null : _mapper.Map<SearchProductDto>(product);
        }
    }
}
