
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RabbitMQOrdering.Api.Entities;
using RabbitMQOrdering.Api.Extensions;

namespace RabbitMQOrdering.Api.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<KitchenArea> KitchenAreas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<PointOfSale> PointsOfSale { get; set; }
        private readonly IEntityFrameworkExtensions _efCoreExtensions;
        public AppDbContext(DbContextOptions<AppDbContext> options, IEntityFrameworkExtensions efCoreExtensions) : base(options)
        {
            _efCoreExtensions = efCoreExtensions;
        }

        public AppDbContext()
        {
        }


        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            // var assemblyName = typeof (IHostBuilder).Assembly.GetName ().Name;

            // string assemblyName = typeof(LogigoShopContext).Namespace;

            optionsBuilder.UseInMemoryDatabase(databaseName: "RDISolution")
                        .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                
            optionsBuilder.EnableSensitiveDataLogging();

            // options.UseSqlServer(connection, b => b.MigrationsAssembly("logigoshop.api.configuration"))
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {

            foreach (var relationship in modelBuilder.Model.GetEntityTypes ().SelectMany (e => e.GetForeignKeys ())) {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            _efCoreExtensions.ApplyConfiguration(modelBuilder);

            _efCoreExtensions.CreateSchemma(modelBuilder);

            _efCoreExtensions.SetTableRelationship(modelBuilder);
            
            base.OnModelCreating (modelBuilder);
        }



        public async Task SeedData()
        {
            List<KitchenArea> areas = new List<KitchenArea>();
            var CreatedDate = DateTime.Now;

            areas.Add(new KitchenArea{
                Name = "Meal",
                CreatedAt = CreatedDate
            });

            areas.Add(new KitchenArea{
                Name = "Beverage",
                CreatedAt = CreatedDate
            });

            areas.Add(new KitchenArea{
                Name = "Side Dish",
                CreatedAt = CreatedDate
            });

            areas.Add(new KitchenArea{
                Name = "Desert",
                CreatedAt = CreatedDate
            });


            var products = new List<Product>();

            products.Add(new Product{
                Id = 1,
                Name = "French Fries",
                Price = 10.00m,
                KitchenAreaId = 3

            });

            products.Add(new Product{
                Id = 2,
                Name = "Cheese burger",
                Price = 13.50m,
                KitchenAreaId = 1

            });

            products.Add(new Product{
                Id = 3,
                Name = "Large Coke",
                Price = 8.00m,
                KitchenAreaId = 2

            });

            products.Add(new Product{
                Id = 4,
                Name = "Ice cream",
                Price = 3.50m,
                KitchenAreaId = 4

            });

            var poinstOfSale = new List<PointOfSale>();

            poinstOfSale.Add(new PointOfSale{
                Id = 1,
                PointOfSaleType = PointOfSaleType.Cashier
            });

            poinstOfSale.Add(new PointOfSale{
                Id = 2,
                PointOfSaleType = PointOfSaleType.SelfServiceTotem
            });

            await this.KitchenAreas.AddRangeAsync(areas);

            await this.Products.AddRangeAsync(products);

            await SaveChangesAsync();
        }

    }
}