using ProductManagement.Application.Queries;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Enums;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using ProductManagement.Application.Queries.QueryHandlers;

namespace ProductTest.ApplicationTests.QueryHandlers
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductQueryService> _mockQueryService;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _mockQueryService = new Mock<IProductQueryService>();
            _handler = new GetProductByIdQueryHandler(_mockQueryService.Object);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsProductDto()
        {
            // Arrange
            var query = new GetProductByIdQuery { Id = Guid.NewGuid() };
            var expectedDto = new ProductDto
            {
                Id = query.Id,
                Name = "Test Product",
                Type = ProductType.Physical,
                Description = "Test Description",
                Quantity = 10
            };

            _mockQueryService.Setup(x => x.GetProductByIdAsync(query.Id))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedDto);
            _mockQueryService.Verify(x => x.GetProductByIdAsync(query.Id), Times.Once);
        }
    }
}