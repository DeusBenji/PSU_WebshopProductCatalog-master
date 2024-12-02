using MediatR;
using System.Collections.Generic;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<IEnumerable<SearchProductDto>>
    {
    }
}
