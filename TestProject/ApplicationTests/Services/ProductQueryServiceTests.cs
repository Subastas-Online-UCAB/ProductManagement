using ProductManagement.Application.Services;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.Enums;
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductTest.ApplicationTests.Services
{
    public class ProductQueryServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductQueryService _service;

        public ProductQueryServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _service = new ProductQueryService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetProductByIdAsync_ExistingId_ReturnsProductDto()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product(
                new ProductId(productId),
                "Test Product",
                ProductType.Physical,
                "Test Description",
                10);

            _mockRepository.Setup(x => x.GetByIdAsync(It.Is<ProductId>(id => id.Value == productId)))
                .ReturnsAsync(product);

            // Act
            var result = await _service.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            result.Name.Should().Be(product.Name);
            result.Type.Should().Be(product.Type);
            result.Description.Should().Be(product.Description);
            result.Quantity.Should().Be(product.Quantity);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsProductListDtos()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product(ProductId.CreateUnique(), "Product 1", ProductType.Physical, "Desc 1", 10),
                new Product(ProductId.CreateUnique(), "Product 2", ProductType.Digital, "Desc 2", 20)
            };

            _mockRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = (await _service.GetAllProductsAsync()).ToList();

            // Assert
            result.Should().HaveCount(2);
            result[0].Name.Should().Be(products[0].Name);
            result[1].Type.Should().Be(products[1].Type);
        }
    }
}