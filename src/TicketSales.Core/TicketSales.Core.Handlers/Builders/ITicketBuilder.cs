using TicketSales.Core.Domain;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Handlers.Builders
{
    public interface ITicketBuilder
    {
        TicketSale CreateDomain(BuyTicket command);

        TicketsSold CreateTicketsSoldEvent(Concert concert, User user, int numberOfTickets);

        TicketsNotSold CreateTicketsNotSoldEvent(Concert concert, User user, TicketsNotSoldReason reason);

        SoldTicketView CreateSoldTicketView(TicketsSold @event);
    }
}
