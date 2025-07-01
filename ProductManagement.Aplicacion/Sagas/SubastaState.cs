using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace ProductManagement.Aplicacion.Sagas
{
    public class ProductoState : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }  // antes Guid
        public string CurrentState { get; set; } = null!;
        public string ProductoId { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }

        // 👉 requerido por MongoDb saga storage
        public int Version { get; set; }
    }
}
