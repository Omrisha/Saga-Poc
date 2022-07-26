﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaPoc.Sagas.Mapping
{
    public class OrderPaymentSagaMap :
        SagaClassMap<OrderPaymentSaga>
    {
        protected override void Configure(EntityTypeBuilder<OrderPaymentSaga> entity, ModelBuilder model)
        {
            entity.Property(x => x.InvoiceDate);
            entity.Property(x => x.Amount);
        }
    }
}
