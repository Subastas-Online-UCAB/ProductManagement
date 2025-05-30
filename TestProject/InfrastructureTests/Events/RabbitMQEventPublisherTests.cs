using Moq;
using RabbitMQ.Client; 
using ProductManagement.Domain.Models;
using ProductManagement.Domain.Interfaces;
using ProductManagement.Infrastructure.Events;
using Xunit;
using Microsoft.EntityFrameworkCore;


namespace ProductTest.InfrastructureTests.Events
{
    public class RabbitMQEventPublisherTests : IDisposable
    {
        private readonly Mock<IConnection> _mockConnection;
        private readonly Mock<RabbitMQ.Client.IModel> _mockChannel; 
        private readonly RabbitMQEventPublisher _publisher;

        public RabbitMQEventPublisherTests()
        {
            _mockChannel = new Mock<RabbitMQ.Client.IModel>(); 
            _mockConnection = new Mock<IConnection>();

            var mockConnectionFactory = new Mock<ConnectionFactory>();
            mockConnectionFactory.Setup(f => f.CreateConnection())
                .Returns(_mockConnection.Object);

            _mockConnection.Setup(c => c.CreateModel())
                .Returns(_mockChannel.Object);

            _publisher = new RabbitMQEventPublisher(mockConnectionFactory.Object);
        }

        [Fact]
        public void PublishProductCreatedAsync_ShouldPublishMessage()
        {
        }

        public void Dispose()
        {
            _publisher.Dispose();
        }
    }
}