using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using MediatR;
using System.Collections.Generic;
using Webshop.Search.Application.Features.SearchCategory.Dtos;

namespace Webshop.Search.Application.Features.SearchCategory.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<SearchCategoryDto>>
    {
    }
}
