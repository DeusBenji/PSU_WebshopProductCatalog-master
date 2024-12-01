﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Features.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
    {
        private ILogger<UpdateCategoryCommandHandler> logger;
        private ICategoryRepository categoryRepository;
        public UpdateCategoryCommandHandler(ILogger<UpdateCategoryCommandHandler> logger, ICategoryRepository categoryRepository)
        {
            this.logger = logger;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.AggregateRoots.Category category = new Domain.AggregateRoots.Category(command.Name);
                category.Description = command.Description;
                category.Id = command.Id;
                await categoryRepository.UpdateAsync(category);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
