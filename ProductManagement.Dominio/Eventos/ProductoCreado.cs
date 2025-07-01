using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Dominio.Eventos
{
    public class ProductoCreado
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public decimal    Cantidad { get; set; }
        public Guid   IdUsuario { get; set; }
    }
}
