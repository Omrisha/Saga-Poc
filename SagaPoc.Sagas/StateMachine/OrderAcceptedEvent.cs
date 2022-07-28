namespace SagaPoc.Sagas.StateMachine
{
    internal class OrderAcceptedEvent
    {
        private Guid correlationId;

        public OrderAcceptedEvent(Guid correlationId)
        {
            this.correlationId = correlationId;
        }

        public DateTime Timestamp { get; set; }
    }
}