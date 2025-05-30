using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Infrastructure.Data;
using ProductManagement.Infrastructure.Repositories;
using Moq;
using Xunit;
using FluentAssertions;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.Enums;

namespace ProductTest.InfrastructureTests.Repositories
{
    public class ProductRepositoryTests
    {
        [Fact]
        public async Task AddAsync_ShouldSaveProduct()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var product = new Product(
                id: new ProductId(Guid.NewGuid()),
                name: "Laptop Gaming",
                type: ProductType.Physical,
                description: "RTX 4090, 32GB RAM",
                quantity: 10
            );

            using (var context = new ProductDbContext(options))
            {
                var repository = new ProductRepository(context, Mock.Of<IEventPublisher>());

                // Act
                await repository.AddAsync(product);

                // Assert
                var savedProduct = await context.Products.FirstOrDefaultAsync();
                savedProduct.Should().NotBeNull();
                savedProduct.Name.Should().Be("Laptop Gaming");
            }
        }

    }
}