using MassTransit;
using SagaPoc.Messages;
using System;
using System.Linq.Expressions;

namespace SagaPoc.Sagas
{
    public class OrderSaga : 
        ISaga, 
        InitiatedBy<SubmitOrder>,
        Orchestrates<OrderAccepted>,
        Observes<OrderShipped, OrderSaga>
    {
        public Guid CorrelationId { get; set; }

        public DateTime? SubmitDate { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime? ShipDate { get; set; }

        public Expression<Func<OrderSaga, OrderShipped, bool>> CorrelationExpression => (saga, message) => saga.CorrelationId == message.OrderId;

        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            SubmitDate = context.Message.OrderDate;
        }

        public async Task Consume(ConsumeContext<OrderAccepted> context)
        {
            AcceptDate = context.Message.Timestamp;
        }
        
        public async Task Consume(ConsumeContext<OrderShipped> context)
        {
            ShipDate = context.Message.ShipDate;
        }
    }
}