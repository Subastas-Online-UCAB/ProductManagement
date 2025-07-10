using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Aplicacion.Dto
{
    public class ProductoCargadoDto
    {
        public Guid IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public decimal Cantidad { get; set; }
        public string ImagenRuta { get; set; }
        public Guid IdUsuario { get; set; }
    }
}
