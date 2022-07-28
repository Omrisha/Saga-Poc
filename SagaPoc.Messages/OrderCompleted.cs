using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaPoc.Messages
{
    public interface OrderCompleted : CorrelatedBy<Guid>
    {
        DateTime Timestamp { get; set; }
    }

    public class OrderCompletedEvent : OrderCompleted
    {
        public OrderCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public DateTime Timestamp { get; set; }

        public Guid CorrelationId { get; }
    }
}
