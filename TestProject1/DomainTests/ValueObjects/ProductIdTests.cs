using ProductManagement.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace ProductTest.DomainTests.ValueObjects
{
    public class ProductIdTests
    {
        [Fact]
        public void Constructor_WithEmptyGuid_ThrowsException()
        {
            // Arrange
            var emptyGuid = Guid.Empty;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ProductId(emptyGuid));
        }

        [Fact]
        public void CreateUnique_CreatesValidProductId()
        {
            // Act
            var productId = ProductId.CreateUnique();

            // Assert
            productId.Value.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public void Equals_WithSameValue_ReturnsTrue()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var productId1 = new ProductId(guid);
            var productId2 = new ProductId(guid);

            // Act & Assert
            productId1.Equals(productId2).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_ForEqualObjects_ReturnsSameValue()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var productId1 = new ProductId(guid);
            var productId2 = new ProductId(guid);

            // Act & Assert
            productId1.GetHashCode().Should().Be(productId2.GetHashCode());
        }
    }
}