using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Dominio.Excepciones
{
    public class UsuarioSinPermisoException : Exception
    {
        public UsuarioSinPermisoException(string userId)
            : base($"El usuario con ID {userId} no tiene permiso para modificar este producto.") { }
    }
}
