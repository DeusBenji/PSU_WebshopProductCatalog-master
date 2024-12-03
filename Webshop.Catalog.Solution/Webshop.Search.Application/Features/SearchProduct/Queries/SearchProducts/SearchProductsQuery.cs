using MediatR;
using System.Collections.Generic;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.SearchProducts
{
    public class SearchProductsQuery : IRequest<IEnumerable<SearchProductDto>>
    {
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SKU { get; set; }

        public SearchProductsQuery() { }

        public SearchProductsQuery(string name, int? categoryId, decimal? minPrice, decimal? maxPrice, string sku)
        {
            Name = name;
            CategoryId = categoryId;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            SKU = sku;
        }
    }
}
