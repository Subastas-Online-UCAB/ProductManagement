using MediatR;
using System.Collections.Generic;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductListDto>>
    {
    }
}