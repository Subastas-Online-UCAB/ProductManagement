using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Domain.Enums;
using System.Threading.Tasks;

namespace ProductManagement.Domain.Interfaces
{
    public interface IProductDomainService
    {
        Task<Product> CreateProductAsync(string name, ProductType type, string description, int quantity);
        Task UpdateProductDetailsAsync(ProductId productId, string name, ProductType type, string description);
        Task AdjustProductQuantityAsync(ProductId productId, int quantityChange);
    }
}