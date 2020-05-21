using Microsoft.EntityFrameworkCore;
using RabbitMQOrdering.Api.Entities;

namespace RabbitMQOrdering.Api.Infrastructure.Data.Relationships
{
    public static class EfCoreRelationshipsExtension
    {
        public static ModelBuilder SetTableRelationships(this ModelBuilder modelBuilder)
        {
            
            
            modelBuilder.Entity<Product> ().HasOne<KitchenArea> (p => p.KitchenArea).WithMany (c => c.Products).HasForeignKey (a => a.Id);

            modelBuilder.Entity<KitchenArea> ().HasMany (a => a.Products).WithOne (p => p.KitchenArea).HasForeignKey (fk => fk.Id);
            

            modelBuilder.Entity<Product> ().HasMany (a => a.ProductOrder).WithOne (c => c.Product).HasForeignKey (pk => pk.ProductId);
            modelBuilder.Entity<Order> ().HasMany (a => a.ProductOrder).WithOne (c => c.Order).HasForeignKey (pk => pk.OrderId);
            
            modelBuilder.Entity<ProductOrder> ().HasKey (pk => pk.Id);
            modelBuilder.Entity<ProductOrder> ().HasOne (cp => cp.Product).WithMany (c => c.ProductOrder).HasForeignKey (cp => cp.ProductId);
            modelBuilder.Entity<ProductOrder> ().HasOne (cp => cp.Order).WithMany (p => p.ProductOrder).HasForeignKey (cp => cp.OrderId);

            modelBuilder.Entity<Order>().HasOne(o => o.PointOfSale).WithMany(p => p.Orders).HasForeignKey(fk => fk.PointOfSaleId);





            /*modelBuilder.Entity<Order> ().HasMany (o => o.OrderDetails).WithOne (od => od.Order).HasForeignKey (a => a.OrderId);
            modelBuilder.Entity<Order> ().HasOne (o => o.OrderPayment).WithOne (b => b.Order).HasForeignKey<OrderPayment> (a => a.OrderId);

            modelBuilder.Entity<OrderDetails> ().HasOne<Order> (od => od.Order).WithMany (c => c.OrderDetails).HasForeignKey (a => a.OrderId);

            modelBuilder.Entity<OrderDetails> ().HasMany (od => od.OrderDetailsProducts).WithOne (c => c.OrderDetails).HasForeignKey (a => a.OrderDetailsId);

            modelBuilder.Entity<OrderPayment> ().HasOne<Order> (od => od.Order).WithOne (c => c.OrderPayment).HasForeignKey<Order> (a => a.Id);
            // modelBuilder.Entity<OrderPayment> ().HasOne<User> (od => od.User).WithOne (c => c.OrderPayment).HasForeignKey<User> (a => a.Id);

            modelBuilder.Entity<OrderReviews> ().HasOne<User> (or => or.User).WithMany (c => c.OrderReviews).HasForeignKey (a => a.UserId);
            modelBuilder.Entity<OrderReviews> ().HasOne<Order> (o => o.Order).WithOne (a => a.OrderReviews).HasForeignKey<Order> (b => b.Id);

            // Partners
            modelBuilder.Entity<Group> ().HasMany (a => a.StoreGroup).WithOne (sg => sg.Group).HasForeignKey (b => b.GroupId);

            modelBuilder.Entity<Partner> ().HasMany (a => a.CategoryPartner).WithOne (p => p.Partner).HasForeignKey (pk => pk.PartnerId);
            modelBuilder.Entity<Partner> ().HasMany (a => a.Products).WithOne (p => p.Partner).HasForeignKey (pk => pk.PartnerId);
            modelBuilder.Entity<Partner> ().HasMany (a => a.Stores).WithOne (s => s.Partner).HasForeignKey (pk => pk.PartnerId);

            modelBuilder.Entity<Store> ().HasMany (a => a.StoreGroup).WithOne (s => s.Store).HasForeignKey (pk => pk.StoreId);
            modelBuilder.Entity<Store> ().HasMany (a => a.Products).WithOne (s => s.Store).HasForeignKey (pk => pk.StoreId);
            modelBuilder.Entity<Store> ().HasMany (a => a.StoreCoupons).WithOne (s => s.Store).HasForeignKey (pk => pk.StoreId);

            modelBuilder.Entity<StoreGroup> ().HasKey (sg => new { sg.StoreId, sg.GroupId });
            modelBuilder.Entity<StoreGroup> ().HasOne (sg => sg.Store).WithMany (s => s.StoreGroup).HasForeignKey (sg => sg.StoreId);
            modelBuilder.Entity<StoreGroup> ().HasOne (sg => sg.Group).WithMany (g => g.StoreGroup).HasForeignKey (sg => sg.GroupId);

            // Products
            modelBuilder.Entity<Categories> ().HasMany (a => a.CategoryPartner).WithOne (c => c.Categories).HasForeignKey (pk => pk.CategoryId);

            modelBuilder.Entity<CategoryPartner> ().HasKey (pk => new { pk.CategoryId, pk.PartnerId });
            modelBuilder.Entity<CategoryPartner> ().HasOne (cp => cp.Categories).WithMany (c => c.CategoryPartner).HasForeignKey (cp => cp.CategoryId);
            modelBuilder.Entity<CategoryPartner> ().HasOne (cp => cp.Partner).WithMany (p => p.CategoryPartner).HasForeignKey (cp => cp.PartnerId);

            modelBuilder.Entity<Product> ().HasOne<Categories> (a => a.Categories).WithMany (p => p.Products).HasForeignKey (fk => fk.CategoryId);
            modelBuilder.Entity<Product> ().HasOne<Partner> (a => a.Partner).WithMany (p => p.Products).HasForeignKey (fk => fk.PartnerId);
            modelBuilder.Entity<Product> ().HasOne<Store> (a => a.Store).WithMany (p => p.Products).HasForeignKey (fk => fk.StoreId);


            modelBuilder.Entity<Product> ().HasMany(a => a.OrderDetailsProducts).WithOne(p =>p.Product).HasForeignKey(fk => fk.ProductId);


            modelBuilder.Entity<Product> ().HasMany (a => a.ProductsAttributes).WithOne (p => p.Product).HasForeignKey (fk => fk.ProductId);
            modelBuilder.Entity<Product> ().HasMany (a => a.ProductsCoupons).WithOne (p => p.Product).HasForeignKey (fk => fk.ProductId);

            modelBuilder.Entity<ProductAttributes> ().HasOne<Product> (a => a.Product).WithMany (pa => pa.ProductsAttributes).HasForeignKey (fk => fk.ProductId);

            modelBuilder.Entity<ShoppingCart> ().HasOne<User> (o => o.User).WithOne (a => a.ShoppingCart).HasForeignKey<ShoppingCart> (b => b.UserId);

            // Promotions
            modelBuilder.Entity<Coupons> ().HasMany (a => a.StoreCoupons).WithOne (c => c.Coupons).HasForeignKey (fk => fk.CouponsId);
            modelBuilder.Entity<Coupons> ().HasMany (a => a.ProductsCoupons).WithOne (c => c.Coupons).HasForeignKey (fk => fk.CouponsId);
            modelBuilder.Entity<Coupons> ().HasMany (a => a.UserCoupons).WithOne (c => c.Coupons).HasForeignKey (fk => fk.CouponsId);

            modelBuilder.Entity<ProductsCoupons> ().HasKey (pk => new { pk.ProductId, pk.CouponsId });
            modelBuilder.Entity<ProductsCoupons> ().HasOne (bc => bc.Product).WithMany (b => b.ProductsCoupons).HasForeignKey (bc => bc.ProductId);
            modelBuilder.Entity<ProductsCoupons> ().HasOne (bc => bc.Coupons).WithMany (c => c.ProductsCoupons).HasForeignKey (bc => bc.CouponsId);

            modelBuilder.Entity<Promotion> ().HasMany (a => a.PromotionsAttributes).WithOne (p => p.Promotion).HasForeignKey (fk => fk.PromotionId);
            modelBuilder.Entity<Promotion> ().HasMany (a => a.UserPromotions).WithOne (p => p.Promotion).HasForeignKey (fk => fk.UserId);

            modelBuilder.Entity<PromotionAttributes> ().HasOne<Promotion> (a => a.Promotion).WithMany (pa => pa.PromotionsAttributes).HasForeignKey (fk => fk.PromotionId);

            modelBuilder.Entity<StoreCoupons> ().HasKey (pk => new { pk.StoreId, pk.CouponsId });
            modelBuilder.Entity<StoreCoupons> ().HasOne (bc => bc.Store).WithMany (b => b.StoreCoupons).HasForeignKey (bc => bc.StoreId);
            modelBuilder.Entity<StoreCoupons> ().HasOne (bc => bc.Coupons).WithMany (c => c.StoreCoupons).HasForeignKey (bc => bc.CouponsId);


            modelBuilder.Entity<OrderDetailsProducts> ().HasKey (pk => new { pk.ProductId, pk.OrderDetailsId });
            modelBuilder.Entity<OrderDetailsProducts> ().HasOne (bc => bc.Product).WithMany (b => b.OrderDetailsProducts).HasForeignKey (bc => bc.ProductId);
            modelBuilder.Entity<OrderDetailsProducts> ().HasOne (bc => bc.OrderDetails).WithMany (c => c.OrderDetailsProducts).HasForeignKey (bc => bc.OrderDetailsId);

            // Users
            modelBuilder.Entity<Address> ().HasOne<User> (a => a.User).WithMany (add => add.Addresses).HasForeignKey (fk => fk.UserId);

            modelBuilder.Entity<UserTemp>().HasOne<User>(ut => ut.User).WithOne(u => u.UserTemp).HasForeignKey<UserTemp>(ut => ut.Id);

            modelBuilder.Entity<User> ().HasOne<OrderPayment> (op => op.OrderPayment).WithOne (u => u.User).HasForeignKey<OrderPayment>(op => op.Id);
            modelBuilder.Entity<User> ().HasMany (a => a.Addresses).WithOne (p => p.User).HasForeignKey (fk => fk.UserId);
            modelBuilder.Entity<User> ().HasMany (a => a.Orders).WithOne (p => p.User).HasForeignKey (fk => fk.UserId);
            modelBuilder.Entity<User> ().HasMany (a => a.UserCards).WithOne (p => p.User).HasForeignKey (fk => fk.UserId);
            modelBuilder.Entity<User> ().HasMany (a => a.UserPromotions).WithOne (p => p.User).HasForeignKey (fk => fk.UserId);
            modelBuilder.Entity<User> ().HasMany (a => a.UserCoupons).WithOne (p => p.User).HasForeignKey (fk => fk.UserId);
            modelBuilder.Entity<User> ().HasMany (a => a.OrderReviews).WithOne (p => p.User).HasForeignKey (fk => fk.UserId);

            modelBuilder.Entity<UserCard> ().HasOne<User> (u => u.User).WithMany (uc => uc.UserCards).HasForeignKey (fk => fk.UserId);

            modelBuilder.Entity<UserCoupons> ().HasKey (pk => new { pk.UserId, pk.CouponsId });
            modelBuilder.Entity<UserCoupons> ().HasOne (uc => uc.User).WithMany (u => u.UserCoupons).HasForeignKey (uc => uc.UserId);
            modelBuilder.Entity<UserCoupons> ().HasOne (uc => uc.Coupons).WithMany (c => c.UserCoupons).HasForeignKey (uc => uc.CouponsId);

            modelBuilder.Entity<UserPromotions> ().HasKey (pk => new { pk.UserId, pk.PromotionId });
            modelBuilder.Entity<UserPromotions> ().HasOne (up => up.User).WithMany (u => u.UserPromotions).HasForeignKey (up => up.UserId);
            modelBuilder.Entity<UserPromotions> ().HasOne (up => up.Promotion).WithMany (p => p.UserPromotions).HasForeignKey (up => up.PromotionId);

            */

            return modelBuilder;
        }
    }
}