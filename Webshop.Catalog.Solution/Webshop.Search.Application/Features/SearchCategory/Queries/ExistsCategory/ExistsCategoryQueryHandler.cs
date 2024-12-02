using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Webshop.Search.Application.Contracts.Persistence;

namespace Webshop.Search.Application.Features.SearchCategory.Queries.ExistsCategory
{
    public class ExistsCategoryQueryHandler : IRequestHandler<ExistsSearchCategoryQuery, bool>
    {
        private readonly ISearchCategoryRepository _searchCategoryRepository;

        public ExistsCategoryQueryHandler(ISearchCategoryRepository categoryRepository)
        {
            _searchCategoryRepository = categoryRepository;
        }

        public async Task<bool> Handle(ExistsSearchCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _searchCategoryRepository.ExistsCategoryAsync(request.ParentId);
        }
    }
}
