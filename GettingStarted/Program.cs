using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using SagaPoc.Sagas;
using Microsoft.EntityFrameworkCore;
using SagaPoc.PersistanceModel;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace GettingStarted
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(hostConfig =>
                {
                    hostConfig.SetBasePath(Directory.GetCurrentDirectory());
                    hostConfig.AddJsonFile("appsettings.json", optional: true);

                })
                .ConfigureServices((hostContext, services) =>
                {
                    var connString = hostContext.Configuration["ConnectionString"];
                    services.AddDbContext<OrderSagaDbContext>((provider, builder) =>
                    {
                        builder.UseSqlServer(connString, m =>
                        {
                            m.MigrationsAssembly("SagaPoc.PersistanceModel");
                            m.MigrationsHistoryTable($"__{nameof(OrderSagaDbContext)}");
                        });
                    });
                    services.AddMassTransit(x =>
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
                        });
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
