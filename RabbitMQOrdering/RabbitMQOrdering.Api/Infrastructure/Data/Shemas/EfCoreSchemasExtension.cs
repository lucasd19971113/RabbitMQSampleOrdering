using Microsoft.EntityFrameworkCore;
using RabbitMQOrdering.Api.Entities;

namespace RabbitMQOrdering.Api.Infrastructure.Data.Shemas
{
    public static class EfCoreSchemasExtension
    {
        public static ModelBuilder CreateSchemmas(this ModelBuilder modelBuilder)
        {

            
            modelBuilder.Entity<Order> ().ToTable ("Orders", "RDI");
            modelBuilder.Entity<Product> ().ToTable ("Products", "RDI");
            modelBuilder.Entity<ProductOrder> ().ToTable ("ProductOrder", "RDI");
            modelBuilder.Entity<KitchenArea> ().ToTable ("KitchenArea", "RDI");
            modelBuilder.Entity<PointOfSale> ().ToTable("PointOfSale", "RDI");
            
            /* 
            modelBuilder.Entity<Partner> ().ToTable ("Partners", "Partners");
            modelBuilder.Entity<Store> ().ToTable ("Stores", "Partners");
            modelBuilder.Entity<Group> ().ToTable ("Groups", "Partners");
            modelBuilder.Entity<StoreGroup> ().ToTable ("StoreGroup", "Partners");

            
            modelBuilder.Entity<Product> ().ToTable ("Products", "Products");
            modelBuilder.Entity<ProductAttributes> ().ToTable ("ProductsAttributes", "Products");
            modelBuilder.Entity<CategoryPartner> ().ToTable ("CategoryPartner", "Products");
            modelBuilder.Entity<Categories> ().ToTable ("Categories", "Products");
            modelBuilder.Entity<ShoppingCart> ().ToTable ("ShoppingCart", "Products");

            
            modelBuilder.Entity<Coupons> ().ToTable ("Coupons", "Promotions");
            modelBuilder.Entity<Promotion> ().ToTable ("Promotions", "Promotions");
            modelBuilder.Entity<PromotionAttributes> ().ToTable ("PromotionsAttributes", "Promotions");
            modelBuilder.Entity<ProductsCoupons> ().ToTable ("ProductsCoupons", "Promotions");
            modelBuilder.Entity<StoreCoupons> ().ToTable ("StoreCoupons", "Promotions");

            
            modelBuilder.Entity<User> ().ToTable ("Users", "Users");
            modelBuilder.Entity<UserTemp>().ToTable("UserTemp", "Users");
            modelBuilder.Entity<AuthenticationQuestions>().ToTable("AuthenticationQuestions", "Users");
            modelBuilder.Entity<Address> ().ToTable ("Address", "Users");
            modelBuilder.Entity<UserPromotions> ().ToTable ("UserPromotions", "Users");
            modelBuilder.Entity<UserCoupons> ().ToTable ("UserCoupons", "Users");
            modelBuilder.Entity<UserCard> ().ToTable ("UserCards", "Users");
            
            */

            return modelBuilder;
        }
    }
}