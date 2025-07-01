using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using ProductManagement.Dominio.Eventos;

namespace ProductManagement.Aplicacion.Sagas
{
    public class ProductoStateMachine : MassTransitStateMachine<ProductoState>
    {
        public State Active { get; private set; } = null!;
        public State Canceled { get; private set; } = null!;

    }
}