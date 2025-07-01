using MongoDB.Driver;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Repositorios;
using ProductManagement.Infraestructura.MongoDB;
using ProductManagement.Infraestructura.MongoDB.Documents;

namespace ProductManagement.Infraestructura.Repositorios
{
    public class MongoProductoRepositorio : IMongoProductoRepositorio
    {
        private readonly IMongoCollection<ProductoDocument> _collection;

        public MongoProductoRepositorio(IProductoMongoContext context)
        {
            _collection = context.Productos;
        }

         public async Task<List<Producto>> ObtenerTodasAsync(CancellationToken cancellationToken)
        {
            var documentos = await _collection.Find(_ => true).ToListAsync(cancellationToken);

            return documentos.Select(doc => new Producto
            {
                IdProducto = doc.Id,
                Nombre = doc.Nombre,
                Descripcion = doc.Descripcion,
                Tipo = doc.Tipo,
                Cantidad = doc.Cantidad,
                IdUsuario = doc.IdUsuario
            }).ToList();

        }
    }
}