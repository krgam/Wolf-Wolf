using TicketSales.Core.Domain;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Handlers.Builders
{
    public interface ITicketBuilder
    {
        TicketSale CreateDomain(BuyTicket command);

        TicketsSold CreateTicketsSoldEvent(Concert concert, User user, int numberOfTickets);

        SoldTicketView CreateSoldTicketView(TicketsSold @event);
    }
}
