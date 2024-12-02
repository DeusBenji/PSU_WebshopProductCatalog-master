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
        private readonly IMapper _mapper;

        public GetChildCategoriesQueryHandler(ISearchCategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchCategoryDto>> Handle(GetChildCategoriesQuery request, CancellationToken cancellationToken)
        {
            var childCategories = await _categoryRepository.GetChildCategoriesAsync(request.ParentId);
            return _mapper.Map<IEnumerable<SearchCategoryDto>>(childCategories);
        }
    }
}