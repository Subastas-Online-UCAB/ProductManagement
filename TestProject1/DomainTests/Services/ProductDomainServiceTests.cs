using ProductManagement.Domain.Services;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Domain.Exceptions;
using Moq;
using FluentAssertions;
using Xunit;
using ProductManagement.Domain.Enums;

namespace ProductTest.DomainTests.Services
{
    public class ProductDomainServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly ProductDomainService _service;
        private readonly ProductId _testId = ProductId.CreateUnique();
        private const string TestName = "Test Product";
        private const ProductType TestType = ProductType.Physical;
        private const string TestDescription = "Test Description";
        private const int TestQuantity = 10;

        public ProductDomainServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _service = new ProductDomainService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateProductAsync_WithValidData_CreatesProduct()
        {
            // Arrange
            _mockRepository.Setup(x => x.ExistsAsync(It.IsAny<ProductId>()))
                .ReturnsAsync(false);

            // Act
            var product = await _service.CreateProductAsync(TestName, TestType, TestDescription, TestQuantity);

            // Assert
            product.Should().NotBeNull();
            product.Name.Should().Be(TestName);
            product.Type.Should().Be(TestType);
            product.Description.Should().Be(TestDescription);
            product.Quantity.Should().Be(TestQuantity);
            _mockRepository.Verify(x => x.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_WithExistingId_ThrowsException()
        {
            // Arrange
            _mockRepository.Setup(x => x.ExistsAsync(It.IsAny<ProductId>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<ProductDomainException>(() =>
                _service.CreateProductAsync(TestName, TestType, TestDescription, TestQuantity));
        }

        [Fact]
        public async Task UpdateProductDetailsAsync_WithExistingProduct_UpdatesProduct()
        {
            // Arrange
            var existingProduct = new Product(_testId, "Old Name", ProductType.Digital, "Old Desc", 5);
            _mockRepository.Setup(x => x.GetByIdAsync(_testId))
                .ReturnsAsync(existingProduct);

            var newName = "New Name";
            var newType = ProductType.Service;
            var newDescription = "New Description";

            // Act
            await _service.UpdateProductDetailsAsync(_testId, newName, newType, newDescription);

            // Assert
            existingProduct.Name.Should().Be(newName);
            existingProduct.Type.Should().Be(newType);
            existingProduct.Description.Should().Be(newDescription);
            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task AdjustProductQuantityAsync_WithPositiveChange_IncreasesQuantity()
        {
            // Arrange
            var existingProduct = new Product(_testId, TestName, TestType, TestDescription, TestQuantity);
            _mockRepository.Setup(x => x.GetByIdAsync(_testId))
                .ReturnsAsync(existingProduct);

            var increaseAmount = 5;

            // Act
            await _service.AdjustProductQuantityAsync(_testId, increaseAmount);

            // Assert
            existingProduct.Quantity.Should().Be(TestQuantity + increaseAmount);
            _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}