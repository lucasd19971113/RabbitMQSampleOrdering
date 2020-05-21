using Microsoft.EntityFrameworkCore;
using RabbitMQOrdering.Api.Infrastructure.Data.Mappings;

namespace RabbitMQOrdering.Api.ContextInfrastructure.Data.Mappings
{
    public static class EfCoreMappingsExtension
    {
        public static ModelBuilder ApplyConfigurations(this ModelBuilder modelBuilder){

            
            
            modelBuilder.ApplyConfiguration (new OrderMapping ());
            modelBuilder.ApplyConfiguration (new KitchenAreaMapping ());
            modelBuilder.ApplyConfiguration (new ProductMapping ());


            /*
            modelBuilder.ApplyConfiguration (new OrderPaymentMap ());
            // modelBuilder.ApplyConfiguration(new PaymentSettingsMap());

     

            modelBuilder.ApplyConfiguration (new GroupsMap ());
            modelBuilder.ApplyConfiguration (new PartnerMap ());
            modelBuilder.ApplyConfiguration (new StoreMap ());

           

            modelBuilder.ApplyConfiguration (new ProductMap ());
            modelBuilder.ApplyConfiguration (new ProductAttributesMap ());
            modelBuilder.ApplyConfiguration (new CategoriesMap ());
            modelBuilder.ApplyConfiguration (new ShoppingCartMap ());

            

            modelBuilder.ApplyConfiguration (new CouponsMap ());
            modelBuilder.ApplyConfiguration (new PromotionMap ());
            modelBuilder.ApplyConfiguration (new PromotionAttributesMap ());

            

            modelBuilder.ApplyConfiguration (new UserMap ());
            modelBuilder.ApplyConfiguration(new UserTempMap());
            modelBuilder.ApplyConfiguration(new AuthenticationQuestionsMap());
            modelBuilder.ApplyConfiguration (new AddressMap ());
            modelBuilder.ApplyConfiguration (new UserCardMap ());

            */

            return modelBuilder;
        }
    }

    public interface IEfCoreMappingsExtension
    {
        ModelBuilder ApplyConfigutions(ModelBuilder modelBuilder);
    }
}