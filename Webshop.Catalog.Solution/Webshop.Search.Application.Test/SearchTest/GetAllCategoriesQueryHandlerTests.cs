using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Application.Features.SearchCategory.Dtos;
using Webshop.Search.Application.Features.SearchCategory.Queries;

namespace Webshop.Search.Application.Test.SearchTest
{
    public class GetAllCategoriesQueryHandlerTests
    {
        [Fact]
        public async Task TestGetAllCategoriesQueryHandler_ValidRequest_ReturnsCategories()
        {
            // Arrange
            var mockCategoryRepository = new Mock<ISearchCategoryRepository>();
            mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync())
                .ReturnsAsync(new List<Domain.AggregateRoots.Category>
                {
            new Domain.AggregateRoots.Category("Category 1"), // Brug konstruktøren
            new Domain.AggregateRoots.Category("Category 2")  // Brug konstruktøren
                });

            var handler = new GetAllCategoriesQueryHandler(mockCategoryRepository.Object);
            var query = new GetAllCategoriesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "Category 1");
        }
    }
}