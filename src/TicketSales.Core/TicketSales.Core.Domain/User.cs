namespace TicketSales.Core.Domain
{
    public class User
    {
        public int Id { get; }

        public string Name { get; }

        /// <summary>
        /// Onlyy used by EF data seeding
        /// </summary>        
        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public User(string name)
        {
            Name = name;
        }
    }
}
