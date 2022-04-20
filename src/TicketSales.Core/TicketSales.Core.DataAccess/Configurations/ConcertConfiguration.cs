using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSales.Core.Domain;

namespace TicketSales.Core.DataAccess.Configurations
{
    public class ConcertConfiguration : IEntityTypeConfiguration<Concert>
    {
        public void Configure(EntityTypeBuilder<Concert> builder)
        {
            builder.HasKey(it => it.Id);
            builder.Property(it => it.Id).ValueGeneratedOnAdd();
            builder.Property(it => it.Name).IsRequired();
            builder.Property(it => it.NumberOfTickets).IsRequired();
        }
    }
}

