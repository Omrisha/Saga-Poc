using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaPoc.Messages
{
    public interface OrderAccepted : CorrelatedBy<Guid>
    {
        DateTime Timestamp { get; set; }
    }

    public class OrderSubmittedEvent : OrderAccepted
    {
        public OrderSubmittedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public DateTime Timestamp { get; set; }

        public Guid CorrelationId { get; }
    }
}
