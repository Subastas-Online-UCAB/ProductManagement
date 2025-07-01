using MassTransit;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Infraestructura.Mongo;
using ProductManagement.Infraestructura.MongoDB;
using ProductManagement.Infraestructura.MongoDB.Documents;

namespace ProductManagement.Infraestructura.Consumidor
{
    public class ProductoCreadoConsumidor : IConsumer<ProductoCreado>
    {
        private readonly IProductoMongoContext _context;

        public ProductoCreadoConsumidor(IProductoMongoContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ProductoCreado> context)
        {
            var mensaje = context.Message;

            var documento = new ProductoDocument
            {
                Id = mensaje.Id,
                Nombre = mensaje.Nombre,
                Descripcion = mensaje.Descripcion,
                Tipo = mensaje.Tipo,
                Cantidad = mensaje.Cantidad,
                IdUsuario = mensaje.IdUsuario,
            };

            await _context.Productos.InsertOneAsync(documento);
        }
    }
}