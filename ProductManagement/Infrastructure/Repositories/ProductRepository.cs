using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Infrastructure.Data;
using ProductManagement.Infrastructure.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        private readonly IEventPublisher _eventPublisher;

        public ProductRepository(
            ProductDbContext context,
            IEventPublisher eventPublisher)
        {
            _context = context;
            _eventPublisher = eventPublisher;
        }

        public async Task<Product> GetByIdAsync(ProductId id)
            => await _context.Products.FirstOrDefaultAsync(p => p.Id.Value == id.Value);

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products.ToListAsync();

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            await _eventPublisher.PublishProductCreatedAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            await _eventPublisher.PublishProductUpdatedAsync(product);
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            await _eventPublisher.PublishProductDeletedAsync(product.Id);
        }

        public async Task<bool> ExistsAsync(ProductId id)
            => await _context.Products.AnyAsync(p => p.Id.Value == id.Value);
    }
}