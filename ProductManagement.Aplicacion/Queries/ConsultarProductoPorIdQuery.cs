using MediatR;
using ProductManagement.Aplicacion.Dto;
using ProductManagement.Dominio.Entidades;


namespace ProductManagement.Aplicacion.Queries
{
    public class ConsultarProductoPorIdQuery : IRequest<ProductoCargadoDto?>
    {
        public Guid IdProducto { get; }

        public ConsultarProductoPorIdQuery(Guid idProducto)
        {
            IdProducto = idProducto;
        }
    }
}