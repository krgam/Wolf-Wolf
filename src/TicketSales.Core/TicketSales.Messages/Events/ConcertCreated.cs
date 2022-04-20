namespace TicketSales.Messages.Events
{
    public class ConcertCreated : BaseEvent
    {
        public int ConcertId { get; set; }
    }
}
