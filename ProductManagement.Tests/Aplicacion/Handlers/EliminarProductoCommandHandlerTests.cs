using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Aplicacion.Handlers;
using ProductManagement.Dominio.Repositorios;

namespace SubastaService.Tests.Handlers
{
    public class EliminarProductoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_CuandoLlamadoCorrectamente_DeberiaEliminarProductoYRetornarTrue()
        {
            // Arrange
            var mockRepo = new Mock<IAuctionRepository>();
            var handler = new EliminarProductoCommandHandler(mockRepo.Object);

            var command = new EliminarProductoCommand
            {
                IdProducto = Guid.NewGuid(),
                IdUsuario = Guid.NewGuid()
            };

            mockRepo
                .Setup(r => r.EliminarProductoAsync(command.IdProducto, command.IdUsuario, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask); // como es void, solo simulamos Task

            // Act
            var resultado = await handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().BeTrue();

            mockRepo.Verify(r =>
                    r.EliminarProductoAsync(command.IdProducto, command.IdUsuario, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}