using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Aplicacion.Commands
{
    public class EliminarProductoCommand : IRequest<bool>
    {
        public Guid IdProducto { get; set; }
        public Guid IdUsuario { get; set; }
    }
}
