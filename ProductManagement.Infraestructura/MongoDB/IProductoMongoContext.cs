using MongoDB.Driver;
using ProductManagement.Infraestructura.MongoDB.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infraestructura.MongoDB
{
    public interface IProductoMongoContext
    {
        IMongoCollection<ProductoDocument> Productos { get; }
    }
}
