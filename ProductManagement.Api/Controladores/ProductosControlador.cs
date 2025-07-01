using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Aplicacion.Queries;

namespace ProductManagement.Api.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosControlador : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductosControlador(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProductoCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(new { Id = id, Mensaje = "Producto recuperado (placeholder)" });
        }


        [HttpPut("editar")]
        public async Task<IActionResult> EditarProducto([FromBody] EditarProductoCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("eliminar/{id}")]
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

        [HttpGet("buscar/{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
        {
            var resultado = await _mediator.Send(new ConsultarProductoPorIdQuery(id), cancellationToken);

            if (resultado is null)
                return NotFound();

            return Ok(resultado);
        }


    }
}
