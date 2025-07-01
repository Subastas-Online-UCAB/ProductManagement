using MediatR;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Repositorios;
using ProductManagement.Dominio.Interfaces;
using ProductManagement.Dominio.Eventos;

namespace ProductManagement.Aplicacion.Servicios
{
    public class CrearProductoCommandHandler : IRequestHandler<CrearProductoCommand, Guid>
    {
        private readonly IAuctionRepository _repository;
        private readonly IPublicadorProductoEventos _publisher;

        public CrearProductoCommandHandler(IAuctionRepository repository, IPublicadorProductoEventos publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<Guid> Handle(CrearProductoCommand request, CancellationToken cancellationToken)
        {
            var producto = new Producto
            {
                IdProducto = Guid.NewGuid(),
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Tipo = request.Tipo,
                Cantidad = request.Cantidad,
                IdUsuario = request.IdUsuario,
            };

            // 1. Persistencia en base de datos principal (PostgreSQL)
            await _repository.CrearAsync(producto, cancellationToken);

            // 2. Publicar evento general (por ejemplo, para vistas o proyecciones)
            var eventoCreado = new ProductoCreado
            {
                Id = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Tipo = producto.Tipo,
                Cantidad = producto.Cantidad,
                IdUsuario = producto.IdUsuario,
            };

            await _publisher.PublicarProductoCreado(eventoCreado);


            return producto.IdProducto;
        }
    }
}
