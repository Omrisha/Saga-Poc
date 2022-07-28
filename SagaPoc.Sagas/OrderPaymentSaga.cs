using MassTransit;
using SagaPoc.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaPoc.Sagas
{
    public class OrderPaymentSaga :
        ISaga,
        InitiatedByOrOrchestrates<OrderInoviced>
    {
        public Guid CorrelationId { get; set; }

        public DateTime? InvoiceDate { get; set; }
        public decimal? Amount { get; set; }

        public async Task Consume(ConsumeContext<OrderInoviced> context)
        {
            if (context.MessageId.HasValue)
                CorrelationId = context.MessageId.Value;
            InvoiceDate = context.Message.Timestamp;
            Amount = context.Message.Amount;

            await context.RespondAsync<OrderPaymentResult>(new
            {
                CorrelationId = CorrelationId,
                Timestame = InvoiceDate,
                Amount = Amount
            });
        }
    }
}
