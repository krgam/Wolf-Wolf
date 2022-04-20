using Microsoft.EntityFrameworkCore;
using TicketSales.Core.Domain;

namespace TicketSales.Core.DataAccess.DbContexts
{
    public partial class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Concert> Concerts { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<TicketSale> SoldTickets { get; set; }

        public DbSet<SoldTicketView> SoldTicketsView { get; set; }
    }
}
