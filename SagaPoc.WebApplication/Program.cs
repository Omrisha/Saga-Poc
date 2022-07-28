using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaPoc.Messages;
using SagaPoc.PersistanceModel;
using SagaPoc.Sagas;
using Serilog;
using Serilog.Events;

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

builder.Host.UseSerilog((host, log) =>
{
    if (host.HostingEnvironment.IsProduction())
        log.MinimumLevel.Information();
    else
        log.MinimumLevel.Debug();

    log.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
    log.MinimumLevel.Override("Quartz", LogEventLevel.Information);
    log.WriteTo.Console();
});

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
