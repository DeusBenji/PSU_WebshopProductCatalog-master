using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Features.SearchProduct.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<SearchProductDto>
    {
        public int Id { get; set; }

        public GetProductByIdQuery() { }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
