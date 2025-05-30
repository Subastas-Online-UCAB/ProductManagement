using ProductManagement.Application.Queries;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.DTOs;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace ProductManagement.Application.Queries.QueryHandlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductQueryService _productQueryService;

        public GetProductByIdQueryHandler(IProductQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productQueryService.GetProductByIdAsync(request.Id);
        }
    }
}