using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ProductManagement.Aplicacion.Comun;

namespace ProductManagement.Aplicacion.Commands
{
    public record EditarProductoCommand(
        Guid ProductoId,
        Guid UsuarioId,
        string Nombre,
        string Descripcion,
        string Tipo,
        decimal Cantidad
    ) : IRequest<MessageResponse>;
}
