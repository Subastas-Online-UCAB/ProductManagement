using ProductManagement.Application.Commands;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Domain.ValueObjects;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace ProductManagement.Application.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productId = new ProductId(request.Id);
            var product = await _productRepository.GetByIdAsync(productId);

            if (product != null)
            {
                await _productRepository.DeleteAsync(product);
            }

            return Unit.Value;
        }
    }
}