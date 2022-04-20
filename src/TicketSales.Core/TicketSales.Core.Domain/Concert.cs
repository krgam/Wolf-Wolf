namespace TicketSales.Core.Domain
{
    public class Concert
    {
        public int Id { get; }

        public string Name { get; }

        public int NumberOfTickets { get; private set; }

        public int SoldTickets { get; private set; }

        public void SellTicket(int number)
        {
            this.NumberOfTickets -= number;
            this.SoldTickets += number;
        }

        /// <summary>
        /// Onlyy used by EF data seeding
        /// </summary>        
        public Concert(int id, string name, int numberOfTickets)
        {
            Id = id;
            Name = name;
            NumberOfTickets = numberOfTickets;
        }

        public Concert(string name, int numberOfTickets)
        {
            Name = name;
            NumberOfTickets = numberOfTickets;
        }
    }
}
