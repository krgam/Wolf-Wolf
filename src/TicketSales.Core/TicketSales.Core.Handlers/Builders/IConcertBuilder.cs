using TicketSales.Core.Domain;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Handlers.Builders
{
    public interface IConcertBuilder
    {
        Concert CreateDomain(CreateConcert command);

        ConcertCreated CreateConcertCreatedEvent(Concert concert);
    }
}
