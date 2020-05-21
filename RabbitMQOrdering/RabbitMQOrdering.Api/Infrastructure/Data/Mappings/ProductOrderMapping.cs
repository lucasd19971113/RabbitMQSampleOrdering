using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RabbitMQOrdering.Api.Entities;

namespace RabbitMQOrdering.Api.Infrastructure.Data.Mappings
{
    public class ProductOrderMapping : IEntityTypeConfiguration<ProductOrder>
    {
        public void Configure(EntityTypeBuilder<ProductOrder> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProductQueueStatus)
                .IsRequired()
                .HasColumnName("ProductQueueStatus")
                .HasColumnType("int");
        }
    }
}