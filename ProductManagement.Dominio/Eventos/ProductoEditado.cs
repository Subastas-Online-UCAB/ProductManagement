using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Dominio.Eventos
{
    public class ProductoEditado
    {
        public Guid IdProducto { get; set; }
        public string Nombre { get; set; } = default!;
        public string Descripcion { get; set; } = default!;
        public string Tipo { get; set; } = default!;
        public decimal Cantidad { get; set; }
        public Guid UsuarioId { get; set; }
    }
}

