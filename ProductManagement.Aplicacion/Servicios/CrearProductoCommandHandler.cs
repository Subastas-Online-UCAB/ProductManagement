using MediatR;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Aplicacion.Servicios;
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
        private readonly ImagenService _imagenService;

        public CrearProductoCommandHandler(IAuctionRepository repository, IPublicadorProductoEventos publisher, ImagenService imagenService)
        {
            _repository = repository;
            _publisher = publisher;
            _imagenService = imagenService;
        }

        public async Task<Guid> Handle(CrearProductoCommand request, CancellationToken cancellationToken)
        {
            // 1. Guardar la imagen en el sistema de archivos
            string rutaImagen = await _imagenService.GuardarImagen(request.Imagen, Guid.NewGuid());

            var producto = new Producto
            {
                IdProducto = Guid.NewGuid(),
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Tipo = request.Tipo,
                Cantidad = request.Cantidad,
                ImagenRuta = rutaImagen,
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
                ImagenRuta = producto.ImagenRuta,
                IdUsuario = producto.IdUsuario,
            };

            await _publisher.PublicarProductoCreado(eventoCreado);


            return producto.IdProducto;
        }
    }
}
