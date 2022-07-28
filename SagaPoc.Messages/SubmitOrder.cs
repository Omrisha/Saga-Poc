using MassTransit;

namespace SagaPoc.Messages
{
    public interface SubmitOrder : CorrelatedBy<Guid>
    {
        DateTime OrderDate { get; set; }
    }
}