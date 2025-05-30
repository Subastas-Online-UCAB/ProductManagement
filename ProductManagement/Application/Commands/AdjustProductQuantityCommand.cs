using MediatR;

namespace ProductManagement.Application.Commands
{
    public class AdjustProductQuantityCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public int QuantityChange { get; set; }
    }
}