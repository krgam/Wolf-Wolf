using MassTransit;
using MassTransit.Definition;
using System.Threading.Tasks;
using TicketSales.Core.DataAccess;
using TicketSales.Core.Handlers.Builders;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Handlers.EventHandlers
{
    public class TicketsSoldHandler : IConsumer<TicketsSold>
    {
        private readonly ITicketBuilder builder;
        private readonly ITicketingRepository repository;

        public TicketsSoldHandler(ITicketBuilder builder, ITicketingRepository repository)
        {
            this.builder = builder ?? throw new System.ArgumentNullException(nameof(builder));
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public async Task Consume(ConsumeContext<TicketsSold> context)
        {
            var viewEntity = builder.CreateSoldTicketView(context.Message);

            await repository.InsertSoldTicketView(viewEntity);
        }
    }

    public class TicketsSoldHandlerDefinition : ConsumerDefinition<TicketsSoldHandler>
    {
        public TicketsSoldHandlerDefinition()
        {
            // override the default endpoint name
            EndpointName = "tickets-sold-event";
        }
    }
}
