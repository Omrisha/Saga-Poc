using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaPoc.Sagas.Mapping
{
    public class OrderSagaMap :
        SagaClassMap<OrderSaga>
    {
        protected override void Configure(EntityTypeBuilder<OrderSaga> entity, ModelBuilder model)
        {
            entity.Property(x => x.SubmitDate);
            entity.Property(x => x.AcceptDate);
            entity.Property(x => x.ShipDate);
        }
    }
}
