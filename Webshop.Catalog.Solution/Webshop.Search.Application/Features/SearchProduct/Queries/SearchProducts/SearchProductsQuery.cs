using MediatR;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.SearchProducts
{
    public class SearchProductsQuery : IRequest<IEnumerable<SearchProductDto>>
    {
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public SearchProductsQuery(string name = null, int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            Name = name;
            CategoryId = categoryId;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }
    }
}
