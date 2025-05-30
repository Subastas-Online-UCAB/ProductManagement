using ProductManagement.Application.Commands;
using ProductManagement.Application.Handlers;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;
using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentAssertions;
using ProductManagement.Domain.Enums;

namespace ProductTest.ApplicationTests.CommandHandlers
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new DeleteProductCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ExistingProduct_DeletesProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new DeleteProductCommand { Id = productId };
            var product = new Product(new ProductId(productId), "Test", ProductType.Physical, "Desc", 10);

            _mockRepository.Setup(x => x.GetByIdAsync(It.Is<ProductId>(id => id.Value == productId)))
                .ReturnsAsync(product);
            _mockRepository.Setup(x => x.DeleteAsync(product))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _mockRepository.Verify(x => x.DeleteAsync(product), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistentProduct_CompletesWithoutError()
        {
            // Arrange
            var command = new DeleteProductCommand { Id = Guid.NewGuid() };

            _mockRepository.Setup(x => x.GetByIdAsync(It.IsAny<ProductId>()))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _mockRepository.Verify(x => x.DeleteAsync(It.IsAny<Product>()), Times.Never);
        }
    }
}
