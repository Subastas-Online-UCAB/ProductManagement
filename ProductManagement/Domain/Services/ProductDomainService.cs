using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;
using ProductManagement.Domain.Exceptions;
using System.Threading.Tasks;
using ProductManagement.Domain.Enums;

namespace ProductManagement.Domain.Services
{
    public class ProductDomainService : IProductDomainService
    {
        private readonly IProductRepository _productRepository;

        public ProductDomainService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> CreateProductAsync(string name, ProductType type, string description, int quantity)
        {
            var productId = ProductId.CreateUnique();
            var product = new Product(productId, name, type, description, quantity);

            if (await _productRepository.ExistsAsync(product.Id))
            {
                throw new ProductDomainException($"Product with ID {product.Id} already exists");
            }

            await _productRepository.AddAsync(product);
            return product;
        }

        public async Task UpdateProductDetailsAsync(ProductId productId, string name, ProductType type, string description)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new ProductDomainException($"Product with ID {productId} not found");

            product.SetName(name);
            product.SetType(type);
            product.SetDescription(description);

            await _productRepository.UpdateAsync(product);
        }

        public async Task AdjustProductQuantityAsync(ProductId productId, int quantityChange)
        {
            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new ProductDomainException($"Product with ID {productId} not found");

            if (quantityChange > 0)
            {
                product.IncreaseQuantity(quantityChange);
            }
            else if (quantityChange < 0)
            {
                product.DecreaseQuantity(-quantityChange);
            }

            await _productRepository.UpdateAsync(product);
        }
    }
}
