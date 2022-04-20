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

                        x.AddConsumer<CreateConcertHandler>(typeof(CreateConcertHandlerDefinition));

                        x.SetKebabCaseEndpointNameFormatter();

                        x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            cfg.Host(host, virtualHost, hostConfigurator =>
                            {
                                hostConfigurator.Username(username);
                                hostConfigurator.Password(password);
                            });
                        }));
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
                })
                .Build()
                .RunAsync();

            //    var builder = new ConfigurationBuilder()
            //        .SetBasePath(Directory.GetCurrentDirectory())
            //        .AddJsonFile("appsettings.json", optional: false);

            //    IConfiguration config = builder.Build();

            //    var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        cfg.Host(new Uri(config["MassTransit:RabbitMqUri"]), config["MassTransit:RabbitMqVirtualHost"], hostConfigurator =>
            //        {
            //            hostConfigurator.Username(config["MassTransit:RabbitMqUser"]);
            //            hostConfigurator.Password(config["MassTransit:RabbitMqPassword"]);
            //        });

            //        cfg.ReceiveEndpoint("buy-ticket-commands", e =>
            //        {
            //            e.PrefetchCount = 16;

            //        });
            //    });

            //    var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            //    await busControl.StartAsync(source.Token);
            //    try
            //    {
            //        Console.WriteLine($"Application started {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}");
            //        Console.WriteLine("Press enter to exit");

            //        await Task.Run(() => Console.ReadLine());
            //    }
            //    finally
            //    {
            //        await busControl.StopAsync();
            //    }
        }
    }
}
