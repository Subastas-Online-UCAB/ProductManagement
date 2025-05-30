using ProductManagement.Application.Commands;
using ProductManagement.Application.Handlers;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.ValueObjects;
using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentAssertions;

namespace ProductTest.ApplicationTests.CommandHandlers
{
    public class AdjustProductQuantityCommandHandlerTests
    {
        private readonly Mock<IProductDomainService> _mockDomainService;
        private readonly AdjustProductQuantityCommandHandler _handler;

        public AdjustProductQuantityCommandHandlerTests()
        {
            _mockDomainService = new Mock<IProductDomainService>();
            _handler = new AdjustProductQuantityCommandHandler(_mockDomainService.Object);
        }

        [Fact]
        public async Task Handle_IncreaseQuantity_AdjustsQuantity()
        {
            // Arrange
            var command = new AdjustProductQuantityCommand
            {
                Id = Guid.NewGuid(),
                QuantityChange = 5
            };

            _mockDomainService.Setup(x => x.AdjustProductQuantityAsync(
                    It.Is<ProductId>(id => id.Value == command.Id),
                    command.QuantityChange))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _mockDomainService.VerifyAll();
        }

        [Fact]
        public async Task Handle_DecreaseQuantity_AdjustsQuantity()
        {
            // Arrange
            var command = new AdjustProductQuantityCommand
            {
                Id = Guid.NewGuid(),
                QuantityChange = -3
            };

            _mockDomainService.Setup(x => x.AdjustProductQuantityAsync(
                    It.Is<ProductId>(id => id.Value == command.Id),
                    command.QuantityChange))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _mockDomainService.VerifyAll();
        }
    }
}