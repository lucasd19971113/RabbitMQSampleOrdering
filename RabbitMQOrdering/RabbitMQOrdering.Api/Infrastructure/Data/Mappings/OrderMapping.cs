using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RabbitMQOrdering.Api.Entities;

namespace RabbitMQOrdering.Api.Infrastructure.Data.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(p => p.OrderStatus)
                .IsRequired()
                .HasColumnName("OrderStatus")
                .HasColumnType("int");

            builder.Property(o => o.Total)
                .IsRequired()
                .HasColumnName("OrderTotal")
                .HasColumnType("decimal (18, 2)");

             builder.Property(o => o.CreatedAt)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}