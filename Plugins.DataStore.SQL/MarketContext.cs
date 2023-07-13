using CoreBusiness;
using Microsoft.EntityFrameworkCore;

namespace Plugins.DataStore.SQL
{
    public class MarketContext : DbContext
    {
        public MarketContext(DbContextOptions<MarketContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(c => c.Category)
                .HasForeignKey(p => p.CategoryId);

            // seeding some data
            modelBuilder.Entity<Category>().HasData(
                    new Category { Id = 1, Name = "T-Shirt", Description = "T-Shirt" },
                    new Category { Id = 2, Name = "Pants", Description = "Pants" },
                    new Category { Id = 3, Name = "Shoes", Description = "Shoes" }
                );

            modelBuilder.Entity<Product>().HasData(
                    new Product { Id = 1, CategoryId = 1, Name = "White Shirt", Quantity = 100, Price = 1.99, Details = "details", Description = "Description" },
                    new Product { Id = 2, CategoryId = 1, Name = "Black Shirt", Quantity = 200, Price = 1.99, Details = "details", Description = "Description" },
                    new Product { Id = 3, CategoryId = 2, Name = "Lee", Quantity = 300, Price = 1.50, Details = "details", Description = "Description" },
                    new Product { Id = 4, CategoryId = 2, Name = "BNY", Quantity = 400, Price = 2.50, Details = "details", Description = "Description" },
                    new Product { Id = 5, CategoryId = 3, Name = "W Brown", Quantity = 70, Price = 150, Details = "details", Description = "Description" },
                    new Product { Id = 6, CategoryId = 3, Name = "Nike", Quantity = 160, Price = 75.25, Details = "details", Description = "Description" }
                );
        }
    }
}