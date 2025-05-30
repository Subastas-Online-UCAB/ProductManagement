using ProductManagement.Application.Queries;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.DTOs;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace ProductManagement.Application.Queries.QueryHandlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductListDto>>
    {
        private readonly IProductQueryService _productQueryService;

        public GetAllProductsQueryHandler(IProductQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }

        public async Task<IEnumerable<ProductListDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productQueryService.GetAllProductsAsync();
        }
    }
}