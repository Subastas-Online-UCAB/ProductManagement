using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Infrastructure.Events;
using RabbitMQ.Client;
using ProductManagement.Domain.Interfaces;


namespace ProductManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddSingleton<IEventPublisher>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = configuration["RabbitMQ:HostName"],
                    DispatchConsumersAsync = true 
                };
                return new RabbitMQEventPublisher(factory);
            });

            return services;
        }
    }
}