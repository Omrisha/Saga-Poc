using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaPoc.Messages
{
    public interface OrderPaymentResult
    {
        Guid CorrelationId { get; }
        DateTime Timestamp { get; }
        decimal Amount { get; }
    }
}
