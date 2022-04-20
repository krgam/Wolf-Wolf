namespace TicketSales.Core.Domain
{
    public class SoldTicketView
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int ConcertId { get; set; }

        public string ConcertName { get; set; }

        public int NumberOfTickets { get; set; }
    }
}
