using ProductManagement.Domain.Enums;
using MediatR;

namespace ProductManagement.Application.Commands
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}