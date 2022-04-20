using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSales.Core.Domain;

namespace TicketSales.Core.DataAccess.Configurations
{

    public class SoldTicketViewConfiguration : IEntityTypeConfiguration<SoldTicketView>
    {
        public void Configure(EntityTypeBuilder<SoldTicketView> builder)
        {
            builder.HasNoKey();
            builder.Property(it => it.UserId).IsRequired();
            builder.Property(it => it.UserName).IsRequired();
            builder.Property(it => it.ConcertId).IsRequired();
            builder.Property(it => it.ConcertName).IsRequired();
            builder.Property(it => it.NumberOfTickets).IsRequired();
        }
    }
}

