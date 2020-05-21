using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RabbitMQOrdering.Api.Entities;

namespace RabbitMQOrdering.Api.ContextInfrastructure.Data.Mappings
{
    public class KitchenAreaMapping : IEntityTypeConfiguration<KitchenArea>
    {
        public void Configure(EntityTypeBuilder<KitchenArea> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(k => k.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("nvarchar(40)");

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