using MassTransit;
using SagaPoc.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaPoc.Sagas.StateMachine
{
    public class OrderStateMachine : 
        MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; private set; }
        public State Accepted { get; private set; }
        
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => SubmitOrder, x => x.CorrelateById(context => context.Message.CorrelationId)
            .SelectId(x => NewId.NextGuid()));
            Event(() => OrderAccepted, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(SubmitOrder)
                    .Then(x => 
                    {
                        x.Saga.OrderDate = x.Message.OrderDate;
                        Console.WriteLine("Submitted order.");
                    })
                    .TransitionTo(Submitted)
                    .Publish(context => new OrderSubmittedEvent(context.Saga.CorrelationId) { Timestamp = DateTime.Now }));

            During(Submitted,
                When(OrderAccepted)
                .Then(x => Console.WriteLine("Order accepted."))
                    .TransitionTo(Accepted)
                    .Publish(context => new OrderAcceptedEvent(context.Saga.CorrelationId) { Timestamp = DateTime.Now })
                    .Finalize());
        }

        public Event<SubmitOrder> SubmitOrder { get; private set; }
        public Event<OrderAccepted> OrderAccepted { get; private set; }
    }
}
