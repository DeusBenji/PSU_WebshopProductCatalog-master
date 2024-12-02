using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Search.Application.Features.SearchCategory.Dtos;
using MediatR;

namespace Webshop.Search.Application.Features.SearchCategory.Queries.ExistsCategory
{
    public class ExistsSearchCategoryQuery : IRequest<bool>
    {
        public int ParentId { get; set; }

        public ExistsSearchCategoryQuery(int parentId)
        {
            ParentId = parentId;
        }
    }
}
