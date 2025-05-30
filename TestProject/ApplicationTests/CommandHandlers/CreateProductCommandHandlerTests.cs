using ProductManagement.Application.Commands;
using ProductManagement.Application.Handlers;
using ProductManagement.Domain.Interfaces;  
using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Domain.Enums;
using Moq;
using Xunit;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManagement.ApplicationTests.CommandHandlers
{
    public class CreateProductCommandHandlerTests  
    {
        private readonly Mock<IProductDomainService> _mockDomainService;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _mockDomainService = new Mock<IProductDomainService>();  
            _handler = new CreateProductCommandHandler(_mockDomainService.Object);  
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsProductId()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Type = ProductType.Physical,
                Description = "Test Description",
                Quantity = 10
            };

            var expectedProductId = new ProductId(Guid.NewGuid());
            _mockDomainService.Setup(x => x.CreateProductAsync(
                    command.Name,
                    command.Type,
                    command.Description,
                    command.Quantity))
                .ReturnsAsync(new Product(
                    expectedProductId,
                    command.Name,
                    command.Type,
                    command.Description,
                    command.Quantity));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(expectedProductId.Value);
            _mockDomainService.VerifyAll();
        }
    }
}