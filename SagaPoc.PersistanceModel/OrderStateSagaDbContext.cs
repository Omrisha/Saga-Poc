using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaPoc.Sagas.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaPoc.PersistanceModel
{
    public class OrderStateSagaDbContext :
        SagaDbContext
    {
        public OrderStateSagaDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new OrderStateMap();
            }
        }
    }
}
