using Microsoft.EntityFrameworkCore;
using TicketSales.Core.DataAccess.Configurations;

namespace TicketSales.Core.DataAccess.DbContexts
{
    public partial class TicketingDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new ConcertConfiguration())
                .ApplyConfiguration(new UserConfiguration())
                .ApplyConfiguration(new SoldTicketConfiguration())
                .ApplyConfiguration(new SoldTicketViewConfiguration());

            modelBuilder.SeedTicketingRepository();
        }
    }
}

