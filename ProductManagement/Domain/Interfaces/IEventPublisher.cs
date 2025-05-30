using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishProductCreatedAsync(Product product);
        Task PublishProductUpdatedAsync(Product product);
        Task PublishProductDeletedAsync(ProductId productId);
    }
}