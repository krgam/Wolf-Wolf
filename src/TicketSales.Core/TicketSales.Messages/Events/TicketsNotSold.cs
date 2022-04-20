namespace TicketSales.Messages.Events
{
    public class TicketsNotSold : BaseEvent
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int ConcertId { get; set; }

        public string ConcertName { get; set; }

        public TicketsNotSoldReason Reason { get; set; }
    }

    public enum TicketsNotSoldReason
    {
        NotEnoughTicketsForConcert = 1,
    }
}
