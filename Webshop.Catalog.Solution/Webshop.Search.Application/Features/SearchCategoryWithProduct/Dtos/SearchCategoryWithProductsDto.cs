using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Search.Application.Features.SearchProduct.Dtos;


namespace Webshop.Search.Application.Features.SearchCategoryWithProduct.Dtos
{
    public class SearchCategoryWithProductsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public List<SearchProductDto> Products { get; set; } = new List<SearchProductDto>();
    }
}