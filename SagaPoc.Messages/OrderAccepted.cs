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
        DateTime Timestamp { get; }
    }
}
