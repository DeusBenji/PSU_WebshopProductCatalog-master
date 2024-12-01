using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Application.Features.SearchCategory.Dtos;

namespace Webshop.Search.Application.Features.SearchCategory.Queries.GetChildCategories
{
    public class GetChildCategoriesQueryHandler : IRequestHandler<GetChildCategoriesQuery, IEnumerable<SearchCategoryDto>>
    {
        private readonly ISearchCategoryRepository _categoryRepository;

        public GetChildCategoriesQueryHandler(ISearchCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<SearchCategoryDto>> Handle(GetChildCategoriesQuery request, CancellationToken cancellationToken)
        {
            var childCategories = await _categoryRepository.GetChildCategoriesAsync(request.ParentId);

            return childCategories.Select(category => new SearchCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentId = category.ParentId
            });
        }
    }
}
