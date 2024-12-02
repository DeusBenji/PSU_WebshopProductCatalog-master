using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Search.Application.Features.SearchCategory.Dtos;

using MediatR;


namespace Webshop.Search.Application.Features.SearchCategory.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<SearchCategoryDto>
    {
        public int Id { get; set; }

        public GetCategoryByIdQuery(int id)
        {
            Id = id;
        }
    }
}
