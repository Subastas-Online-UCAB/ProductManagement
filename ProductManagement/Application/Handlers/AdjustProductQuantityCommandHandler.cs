using ProductManagement.Application.Commands;
using ProductManagement.Domain.Services;
using ProductManagement.Domain.ValueObjects;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using ProductManagement.Domain.Interfaces;

public class AdjustProductQuantityCommandHandler : IRequestHandler<AdjustProductQuantityCommand, Unit>
{
    private readonly IProductDomainService _productDomainService;

    public AdjustProductQuantityCommandHandler(IProductDomainService productDomainService)
    {
        _productDomainService = productDomainService;
    }

    public async Task<Unit> Handle(AdjustProductQuantityCommand request, CancellationToken cancellationToken)
    {
        await _productDomainService.AdjustProductQuantityAsync(
            new ProductId(request.Id),
            request.QuantityChange);

        return Unit.Value;
    }
}