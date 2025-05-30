using MediatR;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public Guid Id { get; set; }
    }
}