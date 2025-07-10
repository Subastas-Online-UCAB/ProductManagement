using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Aplicacion.Dto;
using ProductManagement.Dominio.Entidades;

namespace ProductManagement.Aplicacion.Queries
{
    public class GetAllProductosQuery : IRequest<List<Producto>> { }

}
