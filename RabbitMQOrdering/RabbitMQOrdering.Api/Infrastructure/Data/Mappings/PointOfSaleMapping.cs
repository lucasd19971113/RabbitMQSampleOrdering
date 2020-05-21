using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RabbitMQOrdering.Api.Entities;

namespace RabbitMQOrdering.Api.Infrastructure.Data.Mappings
{
    public class PointOfSaleMappimg : IEntityTypeConfiguration<PointOfSale>
    {
        public void Configure(EntityTypeBuilder<PointOfSale> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(p => p.PointOfSaleType)
                .IsRequired()
                .HasColumnName("PointOfSaleType")
                .HasColumnType("int");
        }
    }
}