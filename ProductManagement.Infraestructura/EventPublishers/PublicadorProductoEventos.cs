using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Dominio.Interfaces;

namespace ProductManagement.Infraestructura.EventPublishers
{
    public class PublicadorProductoEventos : IPublicadorProductoEventos
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PublicadorProductoEventos(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublicarProductoCreado(ProductoCreado evento)
        {
            await _publishEndpoint.Publish(evento);
        }


        public async Task PublicarProductoActualizado(ProductoActualizado evento)
        {
            await _publishEndpoint.Publish(evento);
        }
    }
}