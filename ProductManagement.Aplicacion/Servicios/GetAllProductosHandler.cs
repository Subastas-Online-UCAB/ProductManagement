using MediatR;
using ProductManagement.Aplicacion.Queries;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Repositorios;

namespace ProductManagement.Aplicacion.Handlers
{
    public class GetAllProductosHandler : IRequestHandler<GetAllProductosQuery, List<Producto>>
    {
        private readonly IMongoProductoRepositorio _repository;

        public GetAllProductosHandler(IMongoProductoRepositorio repository)
        {
            _repository = repository;
        }

        public async Task<List<Producto>> Handle(GetAllProductosQuery request, CancellationToken cancellationToken)
        {
            var productos = await _repository.ObtenerTodasAsync(cancellationToken);
            return productos;
        }
    }
}