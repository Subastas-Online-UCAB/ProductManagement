using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Aplicacion.Queries;
using ProductManagement.Dominio.Entidades;

namespace ProductManagement.Api.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones relacionadas con subastas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductosController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Crea un producto.
        /// /// <summary>
        /// <param name="command">Datos del producto a crear.</param>
        /// <response code="201">Producto creado exitosamente.</response>
        /// <response code="400">Datos inválidos o incompletos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProductoCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }



        /// <summary>
        /// Obtiene un producto por su ID (placeholder temporal).
        /// /// <summary>
        /// /// <param name="id">ID del producto.</param>
        /// <returns>Objeto con el ID y mensaje de confirmación.</returns>
        /// <response code="200">Producto encontrado (placeholder).</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetById(Guid id)
        {
            return Ok(new { Id = id, Mensaje = "Producto recuperado (placeholder)" });
        }


        /// <summary>
        /// Edita los datos de un prodcuto existente.
        /// /// <summary>
        /// /// <param name="command">Datos actualizados del producto.</param>
        /// <returns>Resultado de la operación.</returns>
        /// <response code="200">Producto editado exitosamente.</response>
        [HttpPut("editar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditarProducto([FromBody] EditarProductoCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        /// <summary>
        /// Elimina una subasta por su ID.
        /// /// <summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <param name="usuarioId">ID del usuario que solicita la eliminación.</param>
        /// <returns>NoContent si fue eliminada, NotFound si no existe.</returns>
        /// <response code="204">Producto eliminado exitosamente.</response>
        /// <response code="404">Producto no encontrado.</response>
        [HttpDelete("eliminar/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid usuarioId)
        {
            var resultado = await _mediator.Send(new EliminarProductoCommand()
            {
                IdProducto = id,
                IdUsuario = usuarioId
            });

            if (!resultado)
                return NotFound();

            return NoContent();
        }


        /// <summary>
        /// Consulta una subasta por su ID.
        /// /// <summary>
        /// /// <param name="id">ID del producto a eliminar.</param>
        /// /// <returns>Detalles del producto si existe.</returns>
        /// /// <response code="200">Producto encontrado.</response>
        /// <response code="404">Producto no encontrado.</response>
        [HttpGet("buscar/{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
        {
            var resultado = await _mediator.Send(new ConsultarProductoPorIdQuery(id), cancellationToken);

            if (resultado is null)
                return NotFound();

            return Ok(resultado);
        }


        /// Obtiene la lista de todos los productos.
        /// </summary>
        /// <remarks>
        /// Retorna todos los productos registrados desde la base de datos de lectura (MongoDB).
        /// No requiere parámetros de entrada.
        /// </remarks>
        /// <returns>Lista de objetos <see cref="Producto"/>.</returns>
        /// <response code="200">Lista de Productos obtenida exitosamente.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var resultado = await _mediator.Send(new GetAllProductosQuery());
            return Ok(resultado);
        }


    }
}
