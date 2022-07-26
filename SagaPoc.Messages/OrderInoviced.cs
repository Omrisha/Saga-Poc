using MassTransit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaPoc.Messages
{
    public interface OrderInoviced : 
        CorrelatedBy<Guid>
    {
        DateTime Timestamp { get; }
        decimal Amount { get; }
    }
}
