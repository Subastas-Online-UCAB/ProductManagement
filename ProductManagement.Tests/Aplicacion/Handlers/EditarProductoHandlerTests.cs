using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Aplicacion.Comun;
using ProductManagement.Aplicacion.Servicios;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Dominio.Interfaces;
using ProductManagement.Dominio.Repositorios;

namespace SubastaService.Tests.Handlers
{
    public class EditarProductoHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsSuccess_WhenProductoIsValidAndEditable()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var productoId = Guid.NewGuid();

            var producto = new Producto
            {
                IdProducto = productoId,
                IdUsuario = usuarioId,
            };

            var mockRepo = new Mock<IAuctionRepository>();
            var mockPublisher = new Mock<IPublicadorProductoEventos>();

            mockRepo
                .Setup(r => r.ObtenerPorIdAsync(productoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(producto);

            mockRepo
                .Setup(r => r.ActualizarAsync(It.IsAny<Producto>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockPublisher
                .Setup(p => p.PublicarProductoEditado(It.IsAny<ProductoEditado>()))
                .Returns(Task.CompletedTask);

            var handler = new EditarProductoHandler(mockRepo.Object, mockPublisher.Object);

            var command = new EditarProductoCommand(
                ProductoId: productoId,
                UsuarioId: usuarioId,
                Nombre: "Nuevo producto",
                Descripcion: "Descripción actualizada",
                Tipo: "Prueba",
                Cantidad: 100
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("Producto editado exitosamente.", result.Message);

            mockRepo.Verify(r => r.ObtenerPorIdAsync(productoId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.ActualizarAsync(producto, It.IsAny<CancellationToken>()), Times.Once);
            mockPublisher.Verify(p => p.PublicarProductoEditado(It.IsAny<ProductoEditado>()), Times.Once);
        }


        [Fact]
        public async Task Handle_ReturnsError_WhenUsuarioNoEsPropietario()
        {
            // Arrange
            var productoId = Guid.NewGuid();
            var usuarioPropietario = Guid.NewGuid();
            var usuarioInvalido = Guid.NewGuid(); // <-- otro usuario

            var producto = new Producto
            {
                IdProducto = productoId,
                IdUsuario = usuarioPropietario,

            };

            var mockRepo = new Mock<IAuctionRepository>();
            var mockPublisher = new Mock<IPublicadorProductoEventos>();

            mockRepo
                .Setup(r => r.ObtenerPorIdAsync(productoId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(producto);

            var handler = new EditarProductoHandler(mockRepo.Object, mockPublisher.Object);

            var command = new EditarProductoCommand(
                ProductoId: productoId,
                UsuarioId: usuarioInvalido, // <-- este no es el dueño
                Nombre: "Hackeo",
                Descripcion: "Intento de editar",
                Tipo: "Maldad",
                Cantidad: 1
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No tienes permiso para editar esta subasta.", result.Message);

            mockRepo.Verify(r => r.ObtenerPorIdAsync(productoId, It.IsAny<CancellationToken>()), Times.Once);
            mockRepo.Verify(r => r.ActualizarAsync(It.IsAny<Producto>(), It.IsAny<CancellationToken>()), Times.Never);
            mockPublisher.Verify(p => p.PublicarProductoEditado(It.IsAny<ProductoEditado>()), Times.Never);
        }


    }
}
