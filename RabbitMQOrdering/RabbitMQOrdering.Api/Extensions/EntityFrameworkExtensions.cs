using Microsoft.EntityFrameworkCore;
using RabbitMQOrdering.Api.ContextInfrastructure.Data.Mappings;
using RabbitMQOrdering.Api.Infrastructure.Data.Relationships;
using RabbitMQOrdering.Api.Infrastructure.Data.Shemas;

namespace RabbitMQOrdering.Api.Extensions
{
    public class EntityFrameworkExtensions : IEntityFrameworkExtensions
    {
        public EntityFrameworkExtensions()
        {
            
        }

        public ModelBuilder ApplyConfiguration(ModelBuilder modelBuilder){

            return  modelBuilder.ApplyConfigurations();
        }

        public ModelBuilder CreateSchemma(ModelBuilder modelBuilder)
        {
            return modelBuilder.CreateSchemmas();
        }

        public ModelBuilder SetTableRelationship(ModelBuilder modelBuilder)
        {
            return modelBuilder.SetTableRelationships();
        }
    }

    public interface IEntityFrameworkExtensions
    {

        ModelBuilder ApplyConfiguration(ModelBuilder modelBuilder);
        ModelBuilder CreateSchemma(ModelBuilder modelBuilder);
        ModelBuilder SetTableRelationship(ModelBuilder modelBuilder);

    }
}