using System.Threading.Tasks;
using System.Collections.Generic;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Interfaces
{
    public interface IProductQueryService
    {
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductListDto>> GetAllProductsAsync();
    }
}