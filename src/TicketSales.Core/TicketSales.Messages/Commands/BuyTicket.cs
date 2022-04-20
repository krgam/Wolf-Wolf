namespace TicketSales.Messages.Commands
{
    public class BuyTicket
    {
        public int UserId { get; set; }

        public int ConcertId { get; set; }

        public int NumberOfTickets { get; set; }
    }
}
