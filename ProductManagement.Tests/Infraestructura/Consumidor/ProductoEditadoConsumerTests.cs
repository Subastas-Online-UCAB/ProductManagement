using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MongoDB.Driver;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Infraestructura.Consumidor;
using ProductManagement.Infraestructura.MongoDB.Documents;
using ProductManagement.Infraestructura.MongoDB;
using ProductManagement.Infraestructura.Consumidores;

namespace ProductManagement.Tests.Consumers
{
    public class ProductoEditadoConsumerTests
    {
        [Fact]
        public async Task Consume_UpdatesSubastaDocumentInMongo_WhenEventIsValid()
        {
            // Arrange
            var evento = new ProductoEditado
            {
                IdProducto = Guid.NewGuid(),
                Nombre = "Producto Editado",
                Descripcion = "Actualizada desde test",
                Tipo = "Actualizada desde test",
                Cantidad = 100,
                UsuarioId = Guid.NewGuid()
            };

            var mockCollection = new Mock<IMongoCollection<ProductoDocument>>();
            var mockContext = new Mock<IProductoMongoContext>();
            var mockConsumeContext = new Mock<ConsumeContext<ProductoEditado>>();

            mockContext.Setup(c => c.Productos).Returns(mockCollection.Object);
            mockConsumeContext.Setup(c => c.Message).Returns(evento);

            var consumer = new ProductoEditadoConsumidor(mockContext.Object);

            // Act
            await consumer.Consume(mockConsumeContext.Object);

            // Assert
            mockCollection.Verify(c => c.ReplaceOneAsync(
                It.Is<FilterDefinition<ProductoDocument>>(f => f != null),
                It.Is<ProductoDocument>(d =>
                    d.Id == evento.IdProducto &&
                    d.Nombre == evento.Nombre &&
                    d.Descripcion == evento.Descripcion &&
                    d.Tipo == evento.Tipo &&
                    d.Cantidad == evento.Cantidad &&
                    d.IdUsuario == evento.UsuarioId
                ),
                It.Is<ReplaceOptions>(o => o.IsUpsert == true),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
