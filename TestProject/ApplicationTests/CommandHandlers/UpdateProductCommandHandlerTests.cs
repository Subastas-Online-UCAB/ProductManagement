using ProductManagement.Application.Commands;
using ProductManagement.Application.Handlers;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Domain.Enums;
using Moq;
using Xunit;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentAssertions;

namespace ProductTest.ApplicationTests.CommandHandlers
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductDomainService> _mockDomainService;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _mockDomainService = new Mock<IProductDomainService>();
            _handler = new UpdateProductCommandHandler(_mockDomainService.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesProduct()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Type = ProductType.Digital,
                Description = "Updated Description"
            };

            
            _mockDomainService.Setup(x => x.UpdateProductDetailsAsync(
                    It.Is<ProductId>(id => id.Value == command.Id),
                    command.Name,
                    command.Type,
                    command.Description))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _mockDomainService.Verify();
        }
    }
}