using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProductManagement.Aplicacion.Handlers;
using ProductManagement.Aplicacion.Queries;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Repositorios;

namespace SubastaService.Tests.Handlers
{
    public class GetAllProductosHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsListOfProductos_WhenDataExists()
        {
            // Arrange
            var mockRepo = new Mock<IMongoProductoRepositorio>();

            var productosMock = new List<Producto>
            {
                new Producto
                {
                    IdProducto = Guid.NewGuid(),
                    Nombre = "Producto 1",
                    Descripcion = "Ejemplo de prueba",
                    Tipo = "Ejemplo de prueba",
                    Cantidad = 100,
                    IdUsuario = Guid.NewGuid(),
                }
            };

            mockRepo.Setup(r => r.ObtenerTodasAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(productosMock);

            var handler = new GetAllProductosHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetAllProductosQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Producto 1", result[0].Nombre);
            mockRepo.Verify(r => r.ObtenerTodasAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}