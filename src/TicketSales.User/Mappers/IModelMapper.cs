using System.Collections.Generic;
using TicketSales.User.Models;

namespace TicketSales.User.Mappers
{
    public interface IModelMapper
    {
        GetUserTicketsResponse Map(IEnumerable<Core.Domain.SoldTicketView> ticketViews);

        GetAvailableConcertsResponse Map(IEnumerable<Core.Domain.Concert> concerts);
    }
}