using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly ImagenService _imagenService;

        public EditarProductoHandler(IAuctionRepository productoRepository, IPublicadorProductoEventos eventPublisher, ImagenService imagenService)
        {
            _productoRepository = productoRepository;
            _eventPublisher = eventPublisher;
            _imagenService = imagenService;

        }

        public async Task<MessageResponse> Handle(EditarProductoCommand request, CancellationToken cancellationToken)
        {
            var producto = await _productoRepository.ObtenerPorIdAsync(request.ProductoId, cancellationToken);
            if (producto == null)
                return MessageResponse.CrearError("El Producto no existe.");

            // 2. Manejar la imagen (si se proporciona una nueva)
            string rutaImagen = producto.ImagenRuta;
            if (request.Imagen != null && request.Imagen.Length > 0)
            {
                // Eliminar la imagen anterior si existe
                if (!string.IsNullOrEmpty(rutaImagen))
                {
                    _imagenService.EliminarImagen(rutaImagen);
                }
                // Guardar la nueva imagen
                rutaImagen = await _imagenService.GuardarImagen(request.Imagen, request.ProductoId);
            }

            // 3. Actualizar el producto
            producto.Editar(
                request.Nombre,
                request.Descripcion,
                request.Cantidad,
                rutaImagen
            );
            producto.Tipo = request.Tipo; // Asignar campo adicional si es necesario

            // 4. Persistir cambios
            await _productoRepository.ActualizarAsync(producto, cancellationToken);

            // 5. Publicar evento de actualización
            await _eventPublisher.PublicarProductoActualizado(new ProductoActualizado
            {
                Id = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Tipo = producto.Tipo,
                Cantidad = producto.Cantidad,
                ImagenRuta = producto.ImagenRuta,
                IdUsuario = producto.IdUsuario
            });


            return MessageResponse.CrearExito("Producto editado exitosamente.");
        }
    }
}
