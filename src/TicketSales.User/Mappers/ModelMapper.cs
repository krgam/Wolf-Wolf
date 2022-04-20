using System;
using System.Collections.Generic;
using System.Linq;
using TicketSales.Core.Domain;
using TicketSales.User.Models;

namespace TicketSales.User.Mappers
{
    public class ModelMapper : IModelMapper
    {
        public GetUserTicketsResponse Map(IEnumerable<SoldTicketView> ticketViews)
        {
            if (ticketViews is null)
            {
                throw new ArgumentNullException(nameof(ticketViews));
            }

            return new GetUserTicketsResponse() { Items = ticketViews.Select(Map).ToList() };
        }

        public GetAvailableConcertsResponse Map(IEnumerable<Core.Domain.Concert> concerts)
        {
            if (concerts is null)
            {
                throw new ArgumentNullException(nameof(concerts));
            }

            return new GetAvailableConcertsResponse() { Items = concerts.Select(Map).ToList() };
        }

        private UserTicket Map(Core.Domain.SoldTicketView item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new UserTicket()
            {
                ConcertName = item.ConcertName,
                NumberOfTickets = item.NumberOfTickets,
            };
        }

        private Models.Concert Map(Core.Domain.Concert item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new Models.Concert()
            {
                Id = item.Id,
                Name = item.Name,
            };
        }
    }
}
