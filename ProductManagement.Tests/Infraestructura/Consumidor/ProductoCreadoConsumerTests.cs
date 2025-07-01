using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using MassTransit;
using MongoDB.Driver;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Infraestructura.Consumidor;
using ProductManagement.Infraestructura.Mongo;
using ProductManagement.Infraestructura.MongoDB.Documents;
using ProductManagement.Infraestructura.MongoDB;

namespace SubastaService.Tests.Consumers
{
    public class ProductoCreadoConsumerTests
    {
        [Fact]
        public async Task Consume_CuandoRecibeEvento_DeberiaInsertarDocumentoEnMongo()
        {
            // Arrange
            var mockCollection = new Mock<IMongoCollection<ProductoDocument>>();
            var mockContext = new Mock<IProductoMongoContext>();

            mockContext.Setup(c => c.Productos).Returns(mockCollection.Object);

            var consumer = new ProductoCreadoConsumidor(mockContext.Object);

            var evento = new ProductoCreado
            {
                Id = Guid.NewGuid(),
                Nombre = "Producto test",
                Descripcion = "Descripción",
                Tipo = "Tipo",
                Cantidad = 100,     
                IdUsuario = Guid.NewGuid(),
            };

            var mockConsumeContext = new Mock<ConsumeContext<ProductoCreado>>();
            mockConsumeContext.Setup(x => x.Message).Returns(evento);

            // Act
            await consumer.Consume(mockConsumeContext.Object);

            // Assert
            mockCollection.Verify(c =>
                c.InsertOneAsync(It.Is<ProductoDocument>(d =>
                    d.Id == evento.Id &&
                    d.Nombre == evento.Nombre &&
                    d.Tipo == "Tipo"
                ),
                null, default),
                Times.Once);
        }
    }
}
