using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;
using RabbitMQ.Client;
using System.Text;
using ProductManagement.Domain.Interfaces;

using System.Text.Json;

namespace ProductManagement.Infrastructure.Events
{
    public class RabbitMQEventPublisher : IEventPublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string ExchangeName = "product_events";

        public RabbitMQEventPublisher(ConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false);
        }

        public Task PublishProductCreatedAsync(Product product)
            => PublishEvent("product_created", product);

        public Task PublishProductUpdatedAsync(Product product)
            => PublishEvent("product_updated", product);

        public Task PublishProductDeletedAsync(ProductId productId)
            => PublishEvent("product_deleted", productId.Value);

        private Task PublishEvent<T>(string eventType, T payload)
        {
            var message = JsonSerializer.Serialize(new
            {
                EventType = eventType,
                Payload = payload,
                Timestamp = DateTime.UtcNow
            });

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: "",
                mandatory: false,
                basicProperties: null,
                body: body);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            GC.SuppressFinalize(this);
        }
    }
}