using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductManagement.Infraestructura.MongoDB;
using ProductManagement.Infraestructura.MongoDB.Documents;

namespace ProductManagement.Infraestructura.Mongo
{
    public class MongoDbContext : IProductoMongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<ProductoDocument> Productos =>
            _database.GetCollection<ProductoDocument>("productos");

        public IMongoDatabase Database => _database;
    }
}
