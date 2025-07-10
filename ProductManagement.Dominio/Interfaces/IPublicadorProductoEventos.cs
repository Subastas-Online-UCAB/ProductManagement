using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Dominio.Eventos;


namespace ProductManagement.Dominio.Interfaces
{
    public interface IPublicadorProductoEventos
    {
        Task PublicarProductoCreado(ProductoCreado evento);
        Task PublicarProductoActualizado(ProductoActualizado evento);
    }
}
