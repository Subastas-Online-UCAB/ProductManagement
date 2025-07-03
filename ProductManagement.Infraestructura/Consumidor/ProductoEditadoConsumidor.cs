using MassTransit;
using MongoDB.Driver;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Infraestructura.MongoDB;
using ProductManagement.Infraestructura.MongoDB.Documents;

namespace ProductManagement.Infraestructura.Consumidores
{
    public class ProductoEditadoConsumidor : IConsumer<ProductoEditado>
    {
        private readonly IProductoMongoContext _mongoContext;

        public ProductoEditadoConsumidor(IProductoMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task Consume(ConsumeContext<ProductoEditado> context)
        {
            var evento = context.Message;

            var filter = Builders<ProductoDocument>.Filter.Eq(s => s.Id, evento.IdProducto);

            var documentoActual = await _mongoContext.Productos
                .Find(filter)
                .FirstOrDefaultAsync();

            var updatedDocument = new ProductoDocument
            {
                Id = evento.IdProducto,
                Nombre = evento.Nombre,
                Descripcion = evento.Descripcion,
                Tipo = evento.Tipo,
                Cantidad = evento.Cantidad,
                IdUsuario = evento.UsuarioId

           
            };

            await _mongoContext.Productos.ReplaceOneAsync(
                filter,
                updatedDocument,
                new ReplaceOptions { IsUpsert = true } // por si aún no existe en Mongo
            );
        }
    }
}