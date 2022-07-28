using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaPoc.Messages;
using SagaPoc.PersistanceModel;
using SagaPoc.Sagas;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration["ConnectionString"];

// Add services to the container.

builder.Services.AddDbContext<OrderSagaDbContext>(options =>
{
    options.UseSqlServer(connString, m =>
    {
        m.MigrationsAssembly("SagaPoc.PersistanceModel");
        m.MigrationsHistoryTable($"__{nameof(OrderSagaDbContext)}");
    });
});

builder.Services.AddMassTransit(x =>
{
    x.AddSaga<OrderSaga>()
    .EntityFrameworkRepository(r =>
    {
        r.ExistingDbContext<OrderSagaDbContext>();
        r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
    });

    x.AddSaga<OrderPaymentSaga>()
    .EntityFrameworkRepository(r =>
    {
        r.ExistingDbContext<OrderSagaDbContext>();
        r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
        cfg.SendTopology.UseCorrelationId<SubmitOrder>(x => x.CorrelationId);
        cfg.SendTopology.UseCorrelationId<OrderAccepted>(x => x.CorrelationId);
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<OrderSagaDbContext>();
await dbContext.Database.EnsureCreatedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
