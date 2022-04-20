using System.Collections.Generic;

namespace TicketSales.User.Models
{
    public class GetUserTicketsResponse
    {
        public List<UserTicket> Items { get; set; }
    }

    public class UserTicket
    {
        public string ConcertName { get; set; }

        public int NumberOfTickets { get; set; }
    }
}
