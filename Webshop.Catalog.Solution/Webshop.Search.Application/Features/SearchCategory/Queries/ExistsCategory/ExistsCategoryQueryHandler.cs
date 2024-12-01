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
    public class ExistsCategoryQueryHandler : IRequestHandler<ExistsCategoryQuery, bool>
    {
        private readonly ISearchCategoryRepository _categoryRepository;

        public ExistsCategoryQueryHandler(ISearchCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> Handle(ExistsCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.ExistsCategoryAsync(request.ParentId);
        }
    }
}
