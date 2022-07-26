using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaPoc.Sagas.Mapping;

namespace SagaPoc.PersistanceModel
{
    public class OrderSagaDbContext :
        SagaDbContext
    {
        public OrderSagaDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get 
            { 
                yield return new OrderSagaMap();
                yield return new OrderPaymentSagaMap();
            }
        }
    }
}