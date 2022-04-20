namespace TicketSales.Core.Domain
{
    public class TicketSale
    {
        public int UserId { get; }

        public int ConcertId { get; }

        public int NumberOfTickets { get; }

        public TicketSale(int userId, int concertId, int numberOfTickets)
        {
            UserId = userId;
            ConcertId = concertId;
            NumberOfTickets = numberOfTickets;
        }
    }
}
