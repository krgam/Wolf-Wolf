using System.Collections.Generic;

namespace TicketSales.Admin.Models
{
    public class GetConcertsResponse
    {
        public List<Concert> Items { get; set; }
    }

    public class Concert
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SoldTickets { get; set; }
    }
}
