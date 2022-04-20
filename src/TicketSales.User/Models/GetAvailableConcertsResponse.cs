using System.Collections.Generic;

namespace TicketSales.User.Models
{
    public class GetAvailableConcertsResponse
    {
        public List<Concert> Items { get; set; }
    }

    public class Concert
    {
        public int Id { get; set; }

        public string Name { get; set; }        
    }
}
