using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSales.Core.Domain;

namespace TicketSales.Core.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(it => it.Id);
            builder.Property(it => it.Id).ValueGeneratedOnAdd();
            builder.Property(it => it.Name).IsRequired();
        }
    }
}

