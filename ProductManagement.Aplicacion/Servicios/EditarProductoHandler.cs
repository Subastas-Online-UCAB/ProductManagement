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

        public EditarProductoHandler(IAuctionRepository productoRepository, IPublicadorProductoEventos eventPublisher)
        {
            _productoRepository = productoRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<MessageResponse> Handle(EditarProductoCommand request, CancellationToken cancellationToken)
        {
            var producto = await _productoRepository.ObtenerPorIdAsync(request.ProductoId, cancellationToken);
            if (producto == null)
                return MessageResponse.CrearError("El Producto no existe.");

            producto.Editar(request.Nombre, request.Descripcion, request.Cantidad);
            await _productoRepository.ActualizarAsync(producto, cancellationToken);

            await _eventPublisher.PublicarProductoEditado( new ProductoEditado
            {
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Cantidad = producto.Cantidad,
            });

            return MessageResponse.CrearExito("Subasta editada exitosamente.");
        }
    }
}
