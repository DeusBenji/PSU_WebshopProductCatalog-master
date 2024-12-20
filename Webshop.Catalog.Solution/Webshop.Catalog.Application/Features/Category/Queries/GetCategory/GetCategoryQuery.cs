﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Features.Category.Dtos;

namespace Webshop.Catalog.Application.Features.Category.Queries.GetCategory
{
    public class GetCategoryQuery : IQuery<CategoryDto>
    {
        public GetCategoryQuery(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; private set; }
    }
}
