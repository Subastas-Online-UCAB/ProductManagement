using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Aplicacion.Servicios;
using ProductManagement.Dominio.Interfaces;
using ProductManagement.Dominio.Repositorios;
using ProductManagement.Dominio.Eventos;

namespace ProductManagement.Tests.Handlers
{
    public class CreateProductoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeberiaCrearProductoYPublicarEventos()
        {
            // Arrange
            var mockRepo = new Mock<IAuctionRepository>();
            var mockPublisher = new Mock<IPublicadorProductoEventos>();

            var handler = new CrearProductoCommandHandler(mockRepo.Object, mockPublisher.Object);

            var command = new CrearProductoCommand
            {
                Nombre = "Silla Antigua",
                Descripcion = "Silla del periodo celta",
                Tipo = "Mueble",
                Cantidad = 7,
                IdUsuario = Guid.NewGuid(),
            };

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().NotBeEmpty(); // Verifica que el Guid fue generado

            mockRepo.Verify(r => r.CrearAsync(It.IsAny<Dominio.Entidades.Producto>(), It.IsAny<CancellationToken>()), Times.Once);

            mockPublisher.Verify(p => p.PublicarProductoCreado(It.Is<ProductoCreado>(e =>
                e.Nombre == command.Nombre &&
                e.Descripcion == command.Descripcion &&
                e.Tipo == command.Tipo &&
                e.Cantidad == 5 &&
                e.IdUsuario == command.IdUsuario
            )), Times.Once);
        }
    }
}
