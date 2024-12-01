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

namespace Webshop.Catalog.Application.Features.Category.Queries.GetCategories
{
    public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private ILogger<GetCategoriesQueryHandler> logger;
        private IMapper mapper;
        private ICategoryRepository categoryRepository;
        public GetCategoriesQueryHandler(ILogger<GetCategoriesQueryHandler> logger, IMapper mapper, ICategoryRepository categoryRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                if (query.IncludeChildCategories)
                {
                    var rootCategories = await categoryRepository.GetRootCategories();
                    foreach (var category in rootCategories)
                    {
                        category.ChildCategories = await GetFullyLoadedCategories(category);
                    }
                    //build the dtos
                    List<CategoryDto> resultDtos = new List<CategoryDto>();
                    foreach (var cat in rootCategories)
                    {
                        var catDto = mapper.Map<CategoryDto>(cat);
                        resultDtos.Add(catDto);
                        catDto.Categories = TransformToDto(cat);
                    }
                    return Result.Ok(resultDtos);
                }
                else
                {
                    List<CategoryDto> resultCategories = new List<CategoryDto>();
                    var categories = await categoryRepository.GetRootCategories();
                    foreach (var cat in categories)
                    {
                        resultCategories.Add(mapper.Map<CategoryDto>(cat));
                    }
                    return resultCategories;
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<CategoryDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }

        private async Task<IEnumerable<Domain.AggregateRoots.Category>> GetFullyLoadedCategories(Domain.AggregateRoots.Category parentCategory)
        {
            var categories = await categoryRepository.GetChildCategories(parentCategory.Id);
            foreach (var category in categories)
            {
                category.ChildCategories = await GetFullyLoadedCategories(category);
            }
            return categories;
        }

        private List<CategoryDto> TransformToDto(Domain.AggregateRoots.Category category)
        {
            List<CategoryDto> result = new List<CategoryDto>();
            foreach (var cat in category.ChildCategories)
            {
                var catDto = mapper.Map<CategoryDto>(cat);
                catDto.Categories = TransformToDto(cat);
                result.Add(catDto);
            }
            return result;
        }
    }
}
