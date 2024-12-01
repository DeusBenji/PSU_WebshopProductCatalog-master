using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Search.Application.Features.SearchProduct.Dtos
{
    public class SearchProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public int AmountInStock { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public int MinStock { get; set; }
    }
}
