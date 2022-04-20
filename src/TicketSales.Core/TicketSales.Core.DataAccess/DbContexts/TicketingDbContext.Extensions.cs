using Microsoft.EntityFrameworkCore;
using TicketSales.Core.Domain;

namespace TicketSales.Core.DataAccess.DbContexts
{
    public static class ModelBuilderExtensions
    {
        public static void SeedTicketingRepository(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Concert>(b =>
            {
                b.HasData(new Concert(1, "Concert 1", 10));
                b.HasData(new Concert(2, "Concert 2", 20));
                b.HasData(new Concert(3, "Concert 3", 30));
            });

            modelBuilder.Entity<User>(b =>
            {
                b.HasData(new User(1, "User 1"));
                b.HasData(new User(2, "User 2"));
                b.HasData(new User(3, "User 3"));
            });
        }
    }
}
