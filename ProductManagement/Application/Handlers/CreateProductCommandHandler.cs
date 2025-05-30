using ProductManagement.Domain.Interfaces;  // Add this
using ProductManagement.Application.Commands;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ProductManagement.Application.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductDomainService _productDomainService;  // Changed to interface

        public CreateProductCommandHandler(IProductDomainService productDomainService)  // Changed parameter type
        {
            _productDomainService = productDomainService;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productDomainService.CreateProductAsync(
                request.Name,
                request.Type,
                request.Description,
                request.Quantity);

            return product.Id.Value;
        }
    }
}