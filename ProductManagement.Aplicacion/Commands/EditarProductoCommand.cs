using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using ProductManagement.Aplicacion.Comun;

namespace ProductManagement.Aplicacion.Commands
{
    public class EditarProductoCommand : IRequest<MessageResponse>
    {
        public Guid ProductoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public decimal Cantidad { get; set; }
        public IFormFile Imagen { get; set; } // Opcional: si no se envía, se mantiene la anterior
        public Guid UsuarioId { get; set; }
    }
}
