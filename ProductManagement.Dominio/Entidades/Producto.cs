using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Dominio.Entidades
{
    public class Producto
    {
        public Guid IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public decimal Cantidad { get; set; }
        public Guid  IdUsuario { get; set; }


        public void Editar(string nombre, string descripcion, decimal cantidad)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Cantidad = cantidad;
        }
    }

}
