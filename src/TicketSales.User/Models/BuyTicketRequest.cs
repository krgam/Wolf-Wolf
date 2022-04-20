namespace TicketSales.User.Models
{
    public class BuyTicketRequest
    {
        public int ConcertId { get; set; }

        public int UserId { get; set; }

        public int NumberOfTickets { get; set; }
    }
}