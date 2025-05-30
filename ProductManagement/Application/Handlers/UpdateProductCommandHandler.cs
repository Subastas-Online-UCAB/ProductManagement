using ProductManagement.Application.Commands;
using ProductManagement.Domain.Services;
using ProductManagement.Domain.ValueObjects;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using ProductManagement.Domain.Interfaces;

namespace ProductManagement.Application.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductDomainService _productDomainService;

        public UpdateProductCommandHandler(IProductDomainService productDomainService)
        {
            _productDomainService = productDomainService;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            await _productDomainService.UpdateProductDetailsAsync(
                new ProductId(request.Id),
                request.Name,
                request.Type,
                request.Description);

            return Unit.Value;
        }
    }
}
