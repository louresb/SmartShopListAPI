using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Models;

namespace ShoppingListApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ShoppingListModel> ShoppingLists { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public DbSet<ShoppingCartProduct> ShoppingCartProducts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=app.db;Cache=Shared");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(shoppingCart => shoppingCart.ShoppingList)
                .WithMany(list => list.ShoppingCarts)
                .HasForeignKey(shoppingCart => shoppingCart.ShoppingListId);

            modelBuilder.Entity<ShoppingCartProduct>()
                .HasKey(t => new { t.ShoppingCartId, t.ProductId });

            modelBuilder.Entity<ShoppingCartProduct>()
                .HasOne(pt => pt.ShoppingCart)
                .WithMany(p => p.ShoppingCartProducts)
                .HasForeignKey(pt => pt.ShoppingCartId);

            modelBuilder.Entity<ShoppingCartProduct>()
                .HasOne(pt => pt.Product)
                .WithMany(t => t.ShoppingCartProducts)
                .HasForeignKey(pt => pt.ProductId);
        }
    }
}