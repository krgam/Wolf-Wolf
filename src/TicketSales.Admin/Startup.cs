using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TicketSales.Admin.Mappers;
using TicketSales.Admin.Services;
using TicketSales.Core.DataAccess;
using TicketSales.Core.DataAccess.DbContexts;

namespace TicketSales.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen();

            bool.TryParse(Configuration["UseInMemoryDatabase"], out bool useInMemoryDatabase);
            services.AddDbContext<TicketingDbContext>(options =>
            {
                var sqliteConnString = Configuration["DB_SqliteConnectionString"];
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

            var host = Configuration["MassTransit:RabbitMqUri"];
            var virtualHost = Configuration["MassTransit:RabbitMqVirtualHost"];
            var username = Configuration["MassTransit:RabbitMqUser"];
            var password = Configuration["MassTransit:RabbitMqPassword"];

            services.AddMassTransit(x =>
            {
                //x.UsingInMemory((context, cfg) =>
                //{
                //    cfg.ConfigureEndpoints(context);
                //});

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, virtualHost, h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });
                });
            });

            EndpointConvention.Map<Messages.Commands.CreateConcert>(new Uri($"rabbitmq://{host}/{virtualHost}/create-concert-command"));

            services.AddMassTransitHostedService();

            this.RegisterDependacies(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketSales Admin API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterDependacies(IServiceCollection services)
        {
            services.AddTransient<IAdminService, AdminService>();
            services.AddSingleton<IModelMapper, ModelMapper>();
            services.AddTransient<ITicketingRepository, TicketingRepository>();
        }
    }
}
