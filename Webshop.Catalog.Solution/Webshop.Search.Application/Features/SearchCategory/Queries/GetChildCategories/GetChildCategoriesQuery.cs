using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using MediatR;
using System.Collections.Generic;
using Webshop.Search.Application.Features.SearchCategory.Dtos;

namespace Webshop.Search.Application.Features.SearchCategory.Queries.GetChildCategories
{
    public class GetChildCategoriesQuery : IRequest<IEnumerable<SearchCategoryDto>>
    {
        public int ParentId { get; set; }

        public GetChildCategoriesQuery(int parentId)
        {
            ParentId = parentId;
        }
    }
}
