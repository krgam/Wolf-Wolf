using MassTransit;
using MassTransit.Definition;
using System.Threading.Tasks;
using System.Transactions;
using TicketSales.Core.DataAccess;
using TicketSales.Core.Handlers.Builders;
using TicketSales.Messages.Commands;

namespace TicketSales.Core.Handlers.CommandHandlers
{
    public class CreateConcertHandler : IConsumer<CreateConcert>
    {
        private readonly IConcertBuilder builder;
        private readonly ITicketingRepository repository;

        public CreateConcertHandler(IConcertBuilder builder, ITicketingRepository repository)
        {
            this.builder = builder ?? throw new System.ArgumentNullException(nameof(builder));
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public async Task Consume(ConsumeContext<CreateConcert> context)
        {
            var concert = builder.CreateDomain(context.Message);

            using (var transation = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var concertId = await repository.InsertConcert(concert);

                var @event = builder.CreateConcertCreatedEvent(concert);

                await context.Publish(@event);

                transation.Complete();
            }
        }
    }

    public class CreateConcertHandlerDefinition : ConsumerDefinition<CreateConcertHandler>
    {
        public CreateConcertHandlerDefinition()
        {
            // override the default endpoint name
            EndpointName = "create-concert-command";
        }
    }
}
