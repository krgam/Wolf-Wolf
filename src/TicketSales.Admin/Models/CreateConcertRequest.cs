namespace TicketSales.Admin.Models
{
    public class CreateConcertRequest
    {
        public string ConcertName { get; set; }

        public int NumberOfTickets { get; set; }
    }
}