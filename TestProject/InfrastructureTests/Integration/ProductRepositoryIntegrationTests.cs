using Microsoft.EntityFrameworkCore;
using Moq;
using ProductManagement.Domain.Enums;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Infrastructure.Data;
using ProductManagement.Infrastructure.Events;
using ProductManagement.Infrastructure.Repositories;
using ProductManagement.Domain.Interfaces;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace ProductTest.InfrastructureTests.Integration
{
    public class ProductRepositoryIntegrationTests : IAsyncLifetime
    {
        private ProductDbContext _context;
        private const string ConnectionString = "Host=localhost;Database=TestDB;Username=postgres;Password=admin";

        public async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            _context = new ProductDbContext(options);
            await _context.Database.EnsureCreatedAsync();
        }

        [Fact]
        public async Task AddAsync_ShouldPersistInRealDatabase()
        {
            // Arrange
            var product = new Product(
                ProductId.CreateUnique(),
                "Monitor",
                ProductType.Physical,
                "4K Monitor",
                8);

            var mockEventPublisher = new Mock<IEventPublisher>();
            var repository = new ProductRepository(_context, mockEventPublisher.Object);

            // Act
            await repository.AddAsync(product);

            // Assert
            var savedProduct = await _context.Products.FirstOrDefaultAsync();
            Assert.NotNull(savedProduct);
            Assert.Equal("Monitor", savedProduct.Name);
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
} 
