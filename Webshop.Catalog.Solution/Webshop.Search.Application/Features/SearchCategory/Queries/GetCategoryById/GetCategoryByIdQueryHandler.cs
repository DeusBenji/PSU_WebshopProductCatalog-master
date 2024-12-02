using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Search.Application.Contracts.Persistence;
using MediatR;
using Webshop.Search.Application.Features.SearchCategory.Dtos;

namespace Webshop.Search.Application.Features.SearchCategory.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, SearchCategoryDto>
    {
        private readonly ISearchCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(ISearchCategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<SearchCategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(request.Id);
            if (category == null)
            {
                return null; // Alternativt: kast en exception afhængigt af kravene
            }

            return _mapper.Map<SearchCategoryDto>(category);
        }
    }
}