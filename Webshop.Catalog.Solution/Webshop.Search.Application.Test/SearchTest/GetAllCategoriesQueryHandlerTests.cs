using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Search.Application.Contracts.Persistence;
using Webshop.Search.Application.Features.SearchCategory.Dtos;
using Webshop.Search.Application.Features.SearchCategory.Queries.GetAllCategories;
using AutoMapper;

namespace Webshop.Search.Application.Test.SearchTest
{
    public class GetAllCategoriesQueryHandlerTests
    {
        [Fact]
        public async Task TestGetAllCategoriesQueryHandler_ValidRequest_ReturnsCategories()
        {
            // Arrange
            var mockCategoryRepository = new Mock<ISearchCategoryRepository>();
            var mockMapper = new Mock<IMapper>();

            // Simuler mockdata for kategorier
            var categories = new List<Domain.AggregateRoots.SearchCategory>
            {
                new Domain.AggregateRoots.SearchCategory("Category 1", null) // Brug konstruktøren
                {
                    Description = "Description 1"
                },
                new Domain.AggregateRoots.SearchCategory("Category 2", null) // Brug konstruktøren
                {
                    Description = "Description 2"
                }
            };

            // Mock repository-metode
            mockCategoryRepository.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(categories);

            // Mock mapperen
            mockMapper.Setup(m => m.Map<IEnumerable<SearchCategoryDto>>(categories)).Returns(
                new List<SearchCategoryDto>
                {
                    new SearchCategoryDto { Id = 1, Name = "Category 1", Description = "Description 1", ParentId = null },
                    new SearchCategoryDto { Id = 2, Name = "Category 2", Description = "Description 2", ParentId = null }
                });

            // Opret handler med mock-objekter
            var handler = new GetAllCategoriesQueryHandler(mockCategoryRepository.Object, mockMapper.Object);
            var query = new GetAllCategoriesQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result); // Sikrer, at resultatet ikke er null
            Assert.Equal(2, result.Count()); // Sikrer, at der er to kategorier
            Assert.Contains(result, c => c.Name == "Category 1" && c.Description == "Description 1");
            Assert.Contains(result, c => c.Name == "Category 2" && c.Description == "Description 2");
        }
    }
}
