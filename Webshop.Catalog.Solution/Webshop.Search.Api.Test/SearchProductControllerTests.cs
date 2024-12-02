using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Webshop.Search.Application.Features.SearchProduct.Dtos;
using Webshop.Search.Application.Features.SearchProduct.Queries.GetAllProducts;
using Webshop.Search.Application.Features.SearchProduct.Queries.GetProductById;
using Webshop.Search.Application.Features.SearchProduct.Queries.GetProductsByCategory;
using Webshop.Search.Application.Features.SearchProduct.Queries.SearchProducts;
using Webshop.Search.Api.Controllers;
using MediatR;

public class ProductSearchControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<ProductSearchController>> _loggerMock;
    private readonly ProductSearchController _controller;

    public ProductSearchControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<ProductSearchController>>();
        _controller = new ProductSearchController(_mediatorMock.Object, _loggerMock.Object, null);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsOkResult_WhenProductsExist()
    {
        // Arrange
        var products = new List<SearchProductDto>
        {
            new SearchProductDto { Id = 1, Name = "Product 1", Price = 100 },
            new SearchProductDto { Id = 2, Name = "Product 2", Price = 200 }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _controller.GetAllProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<SearchProductDto>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count());
    }

    [Fact]
    public async Task GetAllProducts_ReturnsNotFound_WhenNoProductsExist()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SearchProductDto>());

        // Act
        var result = await _controller.GetAllProducts();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No products available.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetProductById_ReturnsOkResult_WhenProductExists()
    {
        // Arrange
        var product = new SearchProductDto { Id = 1, Name = "Product 1", Price = 100 };

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProductByIdQuery>(q => q.Id == 1), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.GetProductById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProduct = Assert.IsType<SearchProductDto>(okResult.Value);
        Assert.Equal(1, returnedProduct.Id);
    }

    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SearchProductDto)null);

        // Act
        var result = await _controller.GetProductById(99);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Product with ID 99 not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task SearchProducts_ReturnsOkResult_WhenProductsMatchCriteria()
    {
        // Arrange
        var products = new List<SearchProductDto>
        {
            new SearchProductDto { Id = 1, Name = "Product 1", Price = 100 }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<SearchProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _controller.SearchProducts(name: "Product", categoryId: null, minPrice: null, maxPrice: null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<SearchProductDto>>(okResult.Value);
        Assert.Single(returnedProducts);
    }

    [Fact]
    public async Task SearchProducts_ReturnsNotFound_WhenNoProductsMatchCriteria()
    {
        // Arrange
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<SearchProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SearchProductDto>());

        // Act
        var result = await _controller.SearchProducts(name: "Nonexistent", categoryId: null, minPrice: null, maxPrice: null);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No products matched the search criteria.", notFoundResult.Value);
    }
}
