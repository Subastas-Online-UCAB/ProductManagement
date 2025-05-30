using MediatR;

namespace ProductManagement.Application.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}