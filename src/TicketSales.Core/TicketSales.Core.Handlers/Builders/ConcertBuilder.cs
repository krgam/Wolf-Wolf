using System;
using TicketSales.Core.Domain;
using TicketSales.Messages.Commands;
using TicketSales.Messages.Events;

namespace TicketSales.Core.Handlers.Builders
{
    public class ConcertBuilder : IConcertBuilder
    {
        public Concert CreateDomain(CreateConcert command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            return new Concert(command.ConcertName, command.NumberOfTickets);
        }

        public ConcertCreated CreateConcertCreatedEvent(Concert concert)
        {
            if (concert is null)
            {
                throw new ArgumentNullException(nameof(concert));
            }

            return new ConcertCreated()
            {
                ConcertId = concert.Id,
            };
        }
    }
}
