using MediatR;
using System.Collections.Generic;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryQuery : IRequest<IEnumerable<SearchProductDto>>
    {
        public int CategoryId { get; set; }

        public GetProductsByCategoryQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
