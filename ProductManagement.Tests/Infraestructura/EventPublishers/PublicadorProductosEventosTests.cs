using Xunit;
using Moq;
using FluentAssertions;
using MassTransit;
using System;
using System.Threading.Tasks;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Dominio.Interfaces;
using ProductManagement.Infraestructura.EventPublishers;

namespace ProductManagement.Tests.EventPublishers
{
    public class PublicadorProductosEventosTests
    {
        [Fact]
        public async Task PublicarProductoCreado_DeberiaLlamarPublish()
        {
            // Arrange
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var publisher = new PublicadorProductoEventos(mockEndpoint.Object);

            var evento = new ProductoCreado
            {
                Id = Guid.NewGuid(),
                Nombre = "Nuevo producto",
                Tipo = "Tipo",
                Cantidad = 100,
                IdUsuario = Guid.NewGuid(),
            };

            // Act
            await publisher.PublicarProductoCreado(evento);

            // Assert
            mockEndpoint.Verify(p => p.Publish(evento, default), Times.Once);
        }


        [Fact]
        public async Task PublicarProductoEditado_DeberiaLlamarPublish()
        {
            // Arrange
            var mockEndpoint = new Mock<IPublishEndpoint>();
            var publisher = new PublicadorProductoEventos(mockEndpoint.Object);

            var evento = new ProductoEditado
            {
                IdProducto = Guid.NewGuid(),
                Nombre = "Editada"
            };

            // Act
            await publisher.PublicarProductoEditado(evento);

            // Assert
            mockEndpoint.Verify(p => p.Publish(evento, default), Times.Once);
        }
    }
}
