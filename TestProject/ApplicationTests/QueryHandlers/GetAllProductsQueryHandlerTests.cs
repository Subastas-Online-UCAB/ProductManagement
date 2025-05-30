using ProductManagement.Application.Queries;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.DTOs;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProductManagement.Application.Queries.QueryHandlers;
using ProductManagement.Domain.Enums;

namespace ProductTest.ApplicationTests.QueryHandlers
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductQueryService> _mockQueryService;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _mockQueryService = new Mock<IProductQueryService>();
            _handler = new GetAllProductsQueryHandler(_mockQueryService.Object);
        }

        [Fact]
        public async Task Handle_ReturnsListOfProductDtos()
        {
            // Arrange
            var expectedDtos = new List<ProductListDto>
            {
                new ProductListDto { Id = Guid.NewGuid(), Name = "Product 1", Type = ProductType.Physical, Quantity = 10 },
                new ProductListDto { Id = Guid.NewGuid(), Name = "Product 2", Type = ProductType.Digital, Quantity = 5 }
            };

            _mockQueryService.Setup(x => x.GetAllProductsAsync())
                .ReturnsAsync(expectedDtos);

            var query = new GetAllProductsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedDtos);
            _mockQueryService.Verify(x => x.GetAllProductsAsync(), Times.Once);
        }
    }
}
