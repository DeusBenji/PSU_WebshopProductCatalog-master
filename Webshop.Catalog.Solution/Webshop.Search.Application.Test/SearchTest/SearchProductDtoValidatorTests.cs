using FluentValidation;
using Xunit;
using Webshop.Search.Application.Features.SearchProduct.Dtos;

namespace Webshop.Search.Application.Test.SearchTest
{
    public class SearchProductDtoValidatorTests
    {
        [Fact]
        public void TestSearchProductDto_InvalidNameIsNull_ExpectFailure()
        {
            // Arrange
            var productDto = new SearchProductDto
            {
                Name = null,
                Description = "Valid description",
                SKU = "ABC123",
                AmountInStock = 10,
                Price = 1000,
                Currency = "EUR",
                MinStock = 5
            };

            var validator = new SearchProductDtoValidator();

            // Act
            var validationResult = validator.Validate(productDto);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal("NotEmptyValidator", validationResult.Errors[0].ErrorCode);
            Assert.Equal("Name", validationResult.Errors[0].PropertyName);
        }

        [Fact]
        public void TestSearchProductDto_InvalidPriceNegative_ExpectFailure()
        {
            // Arrange
            var productDto = new SearchProductDto
            {
                Name = "Valid Product",
                Description = "Valid description",
                SKU = "ABC123",
                AmountInStock = 10,
                Price = -100,
                Currency = "EUR",
                MinStock = 5
            };

            var validator = new SearchProductDtoValidator();

            // Act
            var validationResult = validator.Validate(productDto);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal("GreaterThanOrEqualValidator", validationResult.Errors[0].ErrorCode);
            Assert.Equal("Price", validationResult.Errors[0].PropertyName);
        }

        [Fact]
        public void TestSearchProductDto_ValidDto_ExpectSuccess()
        {
            // Arrange
            var productDto = new SearchProductDto
            {
                Name = "Valid Product",
                Description = "Valid description",
                SKU = "ABC123",
                AmountInStock = 10,
                Price = 1000,
                Currency = "EUR",
                MinStock = 5
            };

            var validator = new SearchProductDtoValidator();

            // Act
            var validationResult = validator.Validate(productDto);

            // Assert
            Assert.True(validationResult.IsValid);
        }
    }

    public class SearchProductDtoValidator : AbstractValidator<SearchProductDto>
    {
        public SearchProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Currency).NotEmpty();
        }
    }
}
