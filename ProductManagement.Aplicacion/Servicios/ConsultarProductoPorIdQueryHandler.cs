using MediatR;
using ProductManagement.Aplicacion.Dto;
using ProductManagement.Aplicacion.Queries;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Repositorios;

namespace ProductManagement.Aplicacion.Handlers
{
    public class ConsultarProductoPorIdQueryHandler : IRequestHandler<ConsultarProductoPorIdQuery, ProductoCargadoDto?>
    {
        private readonly IAuctionRepository _repository;

        public ConsultarProductoPorIdQueryHandler(IAuctionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductoCargadoDto?> Handle(ConsultarProductoPorIdQuery request, CancellationToken cancellationToken)
        {
            var producto = await _repository.ObtenerPorIdAsync(request.IdProducto, cancellationToken);

            if (producto == null)
                return null;

            return new ProductoCargadoDto
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion= producto.Descripcion,
                Tipo = producto.Tipo,
                Cantidad = producto.Cantidad,
                IdUsuario = producto.IdUsuario
            };
        }

    }
}