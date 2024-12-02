using FluentValidation;
using Xunit;
using Webshop.Search.Application.Features.SearchCategory.Dtos;

namespace Webshop.Search.Application.Test.SearchTest
{
    public class SearchCategoryDtoValidatorTests
    {
        [Fact]
        public void TestSearchCategoryDto_InvalidNameIsNull_ExpectFailure()
        {
            // Arrange
            var categoryDto = new SearchCategoryDto
            {
                Name = null,
                Description = "Valid description",
                ParentId = 1
            };

            var validator = new SearchCategoryDtoValidator();

            // Act
            var validationResult = validator.Validate(categoryDto);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal("NotEmptyValidator", validationResult.Errors[0].ErrorCode);
            Assert.Equal("Name", validationResult.Errors[0].PropertyName);
        }

        [Fact]
        public void TestSearchCategoryDto_ValidDto_ExpectSuccess()
        {
            // Arrange
            var categoryDto = new SearchCategoryDto
            {
                Name = "Valid Name",
                Description = "Valid description",
                ParentId = 1
            };

            var validator = new SearchCategoryDtoValidator();

            // Act
            var validationResult = validator.Validate(categoryDto);

            // Assert
            Assert.True(validationResult.IsValid);
        }
    }

    public class SearchCategoryDtoValidator : AbstractValidator<SearchCategoryDto>
    {
        public SearchCategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ParentId).GreaterThan(0);
        }
    }
}
