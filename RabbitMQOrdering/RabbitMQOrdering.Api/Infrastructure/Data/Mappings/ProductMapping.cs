using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RabbitMQOrdering.Api.Entities;

namespace RabbitMQOrdering.Api.ContextInfrastructure.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("nvarchar(40)");

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnName("Price")
                .HasColumnType("decimal (18, 2)");

             builder.Property(o => o.CreatedAt)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(o => o.UpdatedAt)
                .HasColumnName("UpdatedDate")
                .HasColumnType("datetime");
        }
    }
}