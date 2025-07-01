using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Dominio.Excepciones
{
    public class ProductoNoEncontradoException : Exception
    {
        public ProductoNoEncontradoException(Guid productoId)
            : base($"No se encontró el producto con ID: {productoId}") { }
    }
}
