using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using System.Threading.Tasks;
using System.Transactions;
using TicketSales.Core.DataAccess;
using TicketSales.Core.Handlers.Builders;
using TicketSales.Messages.Commands;

namespace TicketSales.Core.Handlers.CommandHandlers
{
    public class BuyTicketHandler : IConsumer<BuyTicket>
    {
        private readonly ITicketBuilder builder;
        private readonly ITicketingRepository repository;

        public BuyTicketHandler(ITicketBuilder builder, ITicketingRepository repository)
        {
            this.builder = builder ?? throw new System.ArgumentNullException(nameof(builder));
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public async Task Consume(ConsumeContext<BuyTicket> context)
        {
            var ticketSale = builder.CreateDomain(context.Message);

            var user = await repository.GetUserById(ticketSale.UserId);

            using (var transation = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var concert = await repository.GetConcertById(ticketSale.ConcertId);

                if (concert.NumberOfTickets >= ticketSale.NumberOfTickets)
                {
                    concert.SellTicket(ticketSale.NumberOfTickets);

                    await repository.InsertTicketSale(ticketSale);

                    await repository.UpdateConcert(concert);

                    var @event = builder.CreateTicketsSoldEvent(concert, user, ticketSale.NumberOfTickets);

                    await context.Publish(@event);
                }
                else
                {
                    var @event = builder.CreateTicketsNotSoldEvent(concert, user, Messages.Events.TicketsNotSoldReason.NotEnoughTicketsForConcert);

                    await context.Publish(@event);
                }

                transation.Complete();
            }
        }
    }

    public class BuyTicketHandlerDefinition : ConsumerDefinition<BuyTicketHandler>
    {
        public BuyTicketHandlerDefinition()
        {
            // override the default endpoint name
            EndpointName = "buy-ticket-command";

            // limit the number of messages consumed concurrently
            // this applies to the consumer only, not the endpoint
            ConcurrentMessageLimit = 4;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<BuyTicketHandler> consumerConfigurator)
        {
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
