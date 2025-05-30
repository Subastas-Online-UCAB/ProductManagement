using ProductManagement.Application.Interfaces;
using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Application.Services
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IProductRepository _productRepository;

        public ProductQueryService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(new ProductId(id));

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id.Value,
                Name = product.Name,
                Type = product.Type,
                Description = product.Description,
                Quantity = product.Quantity
            };
        }

        public async Task<IEnumerable<ProductListDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(p => new ProductListDto
            {
                Id = p.Id.Value,
                Name = p.Name,
                Type = p.Type,
                Quantity = p.Quantity
            });
        }
    }
}