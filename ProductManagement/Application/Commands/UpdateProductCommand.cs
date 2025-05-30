using ProductManagement.Domain.Enums;
using MediatR;

namespace ProductManagement.Application.Commands
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public string Description { get; set; }
    }
}