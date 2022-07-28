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

            Event(() => SubmitOrder, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => OrderAccepted, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(SubmitOrder)
                    .Then(x => x.Saga.OrderDate = x.Message.OrderDate)
                    .TransitionTo(Submitted),
                When(OrderAccepted)
                    .TransitionTo(Accepted));

            During(Submitted,
                When(OrderAccepted)
                    .TransitionTo(Accepted));

            During(Accepted,
                When(SubmitOrder)
                    .Then(x => x.Saga.OrderDate = x.Message.OrderDate));
        }

        public Event<SubmitOrder> SubmitOrder { get; private set; }
        public Event<OrderAccepted> OrderAccepted { get; private set; }
    }
}
