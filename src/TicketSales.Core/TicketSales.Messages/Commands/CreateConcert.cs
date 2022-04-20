namespace TicketSales.Messages.Commands
{
    public class CreateConcert
    {
        public string ConcertName { get; set; }

        public int NumberOfTickets { get; set; }
    }
}
