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

namespace Webshop.Search.Api.Tests
{
    public class GetAllCategoriesQueryHandlerTests
    {
        private readonly Mock<ISearchCategoryRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllCategoriesQueryHandler _handler;

        public GetAllCategoriesQueryHandlerTests()
        {
            // Mock repository
            _mockRepository = new Mock<ISearchCategoryRepository>();
            _mockRepository.Setup(repo => repo.GetAllCategoriesAsync())
                .ReturnsAsync(new List<Domain.AggregateRoots.SearchCategory>
                {
                    new Domain.AggregateRoots.SearchCategory("Category 1", null)
                    {
                        Description = "Description 1"
                    },
                    new Domain.AggregateRoots.SearchCategory("Category 2", null)
                    {
                        Description = "Description 2"
                    }
                });

            // Mock AutoMapper
            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<SearchCategoryDto>>(It.IsAny<IEnumerable<Domain.AggregateRoots.SearchCategory>>()))
                .Returns(new List<SearchCategoryDto>
                {
                    new SearchCategoryDto { Id = 1, Name = "Category 1", Description = "Description 1", ParentId = null },
                    new SearchCategoryDto { Id = 2, Name = "Category 2", Description = "Description 2", ParentId = null }
                });

            // Instantiate handler with mocks
            _handler = new GetAllCategoriesQueryHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsCategoryList()
        {
            // Arrange
            var query = new GetAllCategoriesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result); // Check that result is not null
            Assert.Equal(2, result.Count()); // Ensure it contains 2 categories
            Assert.Contains(result, c => c.Name == "Category 1"); // Ensure specific category exists
            Assert.Contains(result, c => c.Name == "Category 2"); // Ensure specific category exists
        }
    }
}
