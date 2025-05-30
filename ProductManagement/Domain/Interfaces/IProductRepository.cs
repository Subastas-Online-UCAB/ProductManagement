using ProductManagement.Domain.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(ProductId id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<bool> ExistsAsync(ProductId id);
    }
}