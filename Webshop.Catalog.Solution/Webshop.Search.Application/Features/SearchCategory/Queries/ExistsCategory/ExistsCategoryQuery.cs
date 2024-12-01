using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Search.Application.Features.Dtos;
using MediatR;

namespace Webshop.Search.Application.Features.SearchCategory.Queries.ExistsCategory
{
    public class ExistsCategoryQuery : IRequest<bool>
    {
        public int ParentId { get; set; }

        public ExistsCategoryQuery(int parentId)
        {
            ParentId = parentId;
        }
    }
}
