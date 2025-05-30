using ProductManagement.Domain.Models;
using ProductManagement.Domain.Enums;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace ProductTest.DomainTests.Models
{
    public class ProductTests
    {
        private readonly ProductId _testId = new ProductId(Guid.NewGuid());
        private const string TestName = "Test Product";
        private const ProductType TestType = ProductType.Physical;
        private const string TestDescription = "Test Description";
        private const int TestQuantity = 10;

        [Fact]
        public void Constructor_WithValidParameters_CreatesProduct()
        {
            // Act
            var product = new Product(_testId, TestName, TestType, TestDescription, TestQuantity);

            // Assert
            product.Id.Should().Be(_testId);
            product.Name.Should().Be(TestName);
            product.Type.Should().Be(TestType);
            product.Description.Should().Be(TestDescription);
            product.Quantity.Should().Be(TestQuantity);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithEmptyName_ThrowsException(string name)
        {
            // Act & Assert
            Assert.Throws<ProductDomainException>(() =>
                new Product(_testId, name, TestType, TestDescription, TestQuantity));
        }

        [Fact]
        public void Constructor_WithNegativeQuantity_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ProductDomainException>(() =>
                new Product(_testId, TestName, TestType, TestDescription, -1));
        }

        [Fact]
        public void SetName_WithValidName_UpdatesName()
        {
            // Arrange
            var product = new Product(_testId, TestName, TestType, TestDescription, TestQuantity);
            var newName = "Updated Product Name";

            // Act
            product.SetName(newName);

            // Assert
            product.Name.Should().Be(newName);
        }

        [Fact]
        public void IncreaseQuantity_WithPositiveAmount_IncreasesQuantity()
        {
            // Arrange
            var product = new Product(_testId, TestName, TestType, TestDescription, TestQuantity);
            var increaseAmount = 5;

            // Act
            product.IncreaseQuantity(increaseAmount);

            // Assert
            product.Quantity.Should().Be(TestQuantity + increaseAmount);
        }

        [Fact]
        public void DecreaseQuantity_WithPositiveAmount_DecreasesQuantity()
        {
            // Arrange
            var product = new Product(_testId, TestName, TestType, TestDescription, TestQuantity);
            var decreaseAmount = 3;

            // Act
            product.DecreaseQuantity(decreaseAmount);

            // Assert
            product.Quantity.Should().Be(TestQuantity - decreaseAmount);
        }

        [Fact]
        public void DecreaseQuantity_BelowZero_ThrowsException()
        {
            // Arrange
            var product = new Product(_testId, TestName, TestType, TestDescription, TestQuantity);
            var decreaseAmount = TestQuantity + 1;

            // Act & Assert
            Assert.Throws<ProductDomainException>(() =>
                product.DecreaseQuantity(decreaseAmount));
        }
    }
}