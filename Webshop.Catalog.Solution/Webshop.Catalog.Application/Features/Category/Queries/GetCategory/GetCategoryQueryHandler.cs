﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Application.Features.Category.Dtos;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Features.Category.Queries.GetCategory
{
    public class GetCategoryQueryHandler : IQueryHandler<GetCategoryQuery, CategoryDto>
    {
        private ILogger<GetCategoryQueryHandler> logger;
        private IMapper mapper;
        private ICategoryRepository categoryRepository;
        public GetCategoryQueryHandler(ILogger<GetCategoryQueryHandler> logger, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result<CategoryDto>> Handle(GetCategoryQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var queryResult = await categoryRepository.GetById(query.CategoryId);
                var categoryDto = mapper.Map<CategoryDto>(queryResult);
                return categoryDto;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<CategoryDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
