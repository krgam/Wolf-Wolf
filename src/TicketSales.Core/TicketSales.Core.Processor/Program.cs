using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using TicketSales.Core.DataAccess.DbContexts;
using TicketSales.Core.Handlers.CommandHandlers;
using Microsoft.Extensions.DependencyInjection;
using TicketSales.Core.DataAccess;
using TicketSales.Core.Handlers.Builders;
using TicketSales.Core.Handlers.EventHandlers;

namespace TicketSales.Core.Processor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            var host = config["MassTransit:RabbitMqUri"];
            var virtualHost = config["MassTransit:RabbitMqVirtualHost"];
            var username = config["MassTransit:RabbitMqUser"];
            var password = config["MassTransit:RabbitMqPassword"];

            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<BuyTicketHandler>(typeof(BuyTicketHandlerDefinition));
                        x.AddConsumer<CreateConcertHandler>(typeof(CreateConcertHandlerDefinition));
                        x.AddConsumer<TicketsSoldHandler>(typeof(TicketsSoldHandlerDefinition));

                        x.SetKebabCaseEndpointNameFormatter();

                        x.UsingInMemory((context, cfg) =>
                        {
                            cfg.ConfigureEndpoints(context);
                        });

                        //x.UsingRabbitMq((context, cfg) =>
                        //{
                        //    cfg.Host(host, virtualHost, h =>
                        //    {
                        //        h.Username(username);
                        //        h.Password(password);
                        //    });
                        //});
                    });

                    bool.TryParse(config["UseInMemoryDatabase"], out bool useInMemoryDatabase);
                    services.AddDbContext<TicketingDbContext>(options =>
                    {
                        string sqliteConnString = config["DB_SqliteConnectionString"];
                        if (useInMemoryDatabase || string.IsNullOrWhiteSpace(sqliteConnString))
                        {
                            options.UseInMemoryDatabase("DB");
                        }
                        else
                        {
                            options.UseSqlite(sqliteConnString);
                        }

                        options.EnableSensitiveDataLogging(true);
                    });

                    RegisterDependacies(services);
                })
                .Build()
                .RunAsync();
        }

        private static void RegisterDependacies(IServiceCollection services)
        {
            services.AddTransient<ITicketingRepository, TicketingRepository>();
            services.AddSingleton<ITicketBuilder, TicketBuilder>();
            services.AddSingleton<IConcertBuilder, ConcertBuilder>();
        }
    }
}
