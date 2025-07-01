using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Dominio.Entidades;

namespace ProductManagement.Dominio.Repositorios
{
    public interface IAuctionRepository
    {
        Task<Guid> CrearAsync(Producto producto, CancellationToken cancellationToken);

        Task<Producto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);
        Task ActualizarAsync(Producto producto, CancellationToken cancellationToken);
        Task EliminarProductoAsync(Guid idProducto, Guid idUsuario, CancellationToken cancellationToken);

        Task<Producto?> ObtenerProductoPorIdAsync(Guid id, CancellationToken cancellationToken);



    }
}
