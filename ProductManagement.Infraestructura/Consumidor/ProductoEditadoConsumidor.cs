using MassTransit;
using MongoDB.Driver;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Infraestructura.MongoDB;
using ProductManagement.Infraestructura.MongoDB.Documents;

namespace SubastaService.Infraestructura.Consumidor
{
    public class ProductoEditadoConsumidor : IConsumer<ProductoActualizado>
    {
        private readonly IProductoMongoContext _mongoContext;

        public ProductoEditadoConsumidor(IProductoMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task Consume(ConsumeContext<ProductoActualizado> context)
        {
            var evento = context.Message;

            var filter = Builders<ProductoDocument>.Filter.Eq(s => s.Id, evento.Id);

            var documentoActual = await _mongoContext.Productos
                .Find(filter)
                .FirstOrDefaultAsync();


            var updatedDocument = new ProductoDocument
            {
                Id = evento.Id,
                Nombre = evento.Nombre,
                Descripcion = evento.Descripcion,
                Tipo = evento.Tipo,
                Cantidad = evento.Cantidad,
                ImagenRuta = evento.ImagenRuta,
                IdUsuario = evento.IdUsuario,
            };

            await _mongoContext.Productos.ReplaceOneAsync(
                filter,
                updatedDocument,
                new ReplaceOptions { IsUpsert = true }
            );
        }
    }
}
