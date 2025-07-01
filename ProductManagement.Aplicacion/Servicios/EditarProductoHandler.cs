using MediatR;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Aplicacion.Comun;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Repositorios;
using ProductManagement.Dominio.Interfaces;

namespace ProductManagement.Aplicacion.Servicios
{
    public class EditarProductoHandler : IRequestHandler<EditarProductoCommand, MessageResponse>
    {
        private readonly IAuctionRepository _productoRepository;
        private readonly IPublicadorProductoEventos _eventPublisher;

        public EditarProductoHandler(IAuctionRepository productoRepository, IPublicadorProductoEventos eventPublisher)
        {
            _productoRepository = productoRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<MessageResponse> Handle(EditarProductoCommand request, CancellationToken cancellationToken)
        {
            // Buscar el producto 
            var producto = await _productoRepository.ObtenerPorIdAsync(request.ProductoId, cancellationToken);
            if (producto == null)
                return MessageResponse.CrearError("El Producto no existe.");

            // Validar que el usuario sea dueño del producto
            if (producto.IdUsuario != request.UsuarioId)
                return MessageResponse.CrearError("No tienes permiso para editar esta subasta.");

            // Aplicar los cambios
            producto.Editar(
                request.Nombre,
                request.Descripcion,
                request.Cantidad
            );

            // Persistir cambios
            await _productoRepository.ActualizarAsync(producto, cancellationToken);

            // Publicar evento si es necesario
            await _eventPublisher.PublicarProductoEditado(new ProductoEditado
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Tipo = producto.Tipo,
                Cantidad = producto.Cantidad,
                UsuarioId = producto.IdUsuario
            });

            return MessageResponse.CrearExito("Producto editado exitosamente.");
        }
    }
}
