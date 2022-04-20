using System;
using TicketSales.Core.Domain;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Handlers.Builders
{
    public class TicketBuilder : ITicketBuilder
    {
        public TicketSale CreateDomain(BuyTicket command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return new TicketSale(command.UserId, command.ConcertId, command.NumberOfTickets);
        }

        public TicketsSold CreateTicketsSoldEvent(Concert concert, User user, int numberOfTickets)
        {
            if (concert is null)
            {
                throw new ArgumentNullException(nameof(concert));
            }

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return new TicketsSold()
            {
                UserId = user.Id,
                UserName = user.Name,
                ConcertId = concert.Id,
                ConcertName = concert.Name,
                NumberOfTickets = numberOfTickets
            };
        }

        public SoldTicketView CreateSoldTicketView(TicketsSold @event)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            return new SoldTicketView()
            {
                ConcertId = @event.ConcertId,
                ConcertName = @event.ConcertName,
                UserId = @event.UserId,
                UserName = @event.UserName,
                NumberOfTickets = @event.NumberOfTickets,
            };
        }

        public TicketsNotSold CreateTicketsNotSoldEvent(Concert concert, User user, TicketsNotSoldReason reason)
        {
            if (concert is null)
            {
                throw new ArgumentNullException(nameof(concert));
            }

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return new TicketsNotSold()
            {
                UserId = user.Id,
                UserName = user.Name,
                ConcertId = concert.Id,
                ConcertName = concert.Name,
                Reason = reason,
            };
        }
    }
}
