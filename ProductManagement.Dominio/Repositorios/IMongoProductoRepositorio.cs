using ProductManagement.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Dominio.Repositorios
{
    public interface IMongoProductoRepositorio
    {
        Task<List<Producto>> ObtenerTodasAsync(CancellationToken cancellationToken);
    }
}
