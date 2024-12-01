using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Search.Application.Features.SearchCategoryWithProduct.Dtos;


namespace Webshop.Search.Application.Features.SearchCategoryWithProduct.Queries
{
    public class SearchCategoryWithProductsQuery : IQuery<SearchCategoryWithProductsDto>
    {
        public SearchCategoryWithProductsQuery(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; private set; }
    }
}
