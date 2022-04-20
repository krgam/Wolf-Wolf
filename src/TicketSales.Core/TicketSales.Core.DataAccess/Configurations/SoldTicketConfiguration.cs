using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSales.Core.Domain;

namespace TicketSales.Core.DataAccess.Configurations
{

    public class SoldTicketConfiguration : IEntityTypeConfiguration<TicketSale>
    {
        public void Configure(EntityTypeBuilder<TicketSale> builder)
        {
            builder.HasKey(it => new { it.ConcertId, it.UserId });
            builder.Property(it => it.NumberOfTickets).IsRequired();
        }
    }
}

