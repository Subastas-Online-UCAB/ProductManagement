using MediatR;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Repositorios;

namespace ProductManagement.Aplicacion.Handler
{ 
    public class EliminarProductoCommandHandler : IRequestHandler<EliminarProductoCommand, bool>
    {
        private readonly IAuctionRepository _repository;

        public EliminarProductoCommandHandler(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(EliminarProductoCommand request, CancellationToken cancellationToken)
        {
            await _repository.EliminarProductoAsync(request.IdProducto, request.IdUsuario, cancellationToken);
            return true;
        }
    }
}
