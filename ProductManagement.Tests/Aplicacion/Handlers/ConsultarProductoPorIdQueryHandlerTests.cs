using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Repositorios;
using ProductManagement.Aplicacion.Handlers;
using ProductManagement.Aplicacion.Queries;
using ProductManagement.Aplicacion.Dto;

namespace ProductManagement.Tests.Handlers
{
    public class ConsultarProductoPorIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_CuandoProductoExiste_DeberiaRetornarDtoCompleto()
        {
            // Arrange
            var mockRepo = new Mock<IAuctionRepository>();
            var handler = new ConsultarProductoPorIdQueryHandler(mockRepo.Object);

            var productoId = Guid.NewGuid();

            var productoFake = new Producto
            {
                IdProducto = productoId,
                Nombre = "Producto Test",
                Descripcion = "Descripción",
                Tipo = "Tipo Test",
                Cantidad = 10,
                IdUsuario = Guid.NewGuid(),
            };

            mockRepo
                .Setup(r => r.ObtenerPorIdAsync(productoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(productoFake);

            var query = new ConsultarProductoPorIdQuery(productoId);

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado!.Id.Should().Be(productoFake.IdProducto);
            resultado.Nombre.Should().Be(productoFake.Nombre);
            resultado.Tipo.Should().Be("Prueba");

            mockRepo.Verify(r => r.ObtenerPorIdAsync(productoId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CuandoProductoNoExiste_DeberiaRetornarNull()
        {
            // Arrange
            var mockRepo = new Mock<IAuctionRepository>();
            var handler = new ConsultarProductoPorIdQueryHandler(mockRepo.Object);

            var idInexistente = Guid.NewGuid();
            var query = new ConsultarProductoPorIdQuery(idInexistente);

            mockRepo
                .Setup(r => r.ObtenerPorIdAsync(idInexistente, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Producto?)null);

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Should().BeNull();

            mockRepo.Verify(r => r.ObtenerPorIdAsync(idInexistente, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
