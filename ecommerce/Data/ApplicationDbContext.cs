using ecommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Cart> Carts { get; set; } // Add this line

        public DbSet<CartItem> CartItems { get; set; } // Add this line
        //public DbSet<Address> Addresses { get; set; } // Add this line



        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Address>().ToTable("Addresses");

            modelBuilder.Entity<CartItem>()
                .HasKey(c => c.CartItemId);

            //    modelBuilder.Entity<Address>()
            //.HasOne(a => a.User)
            //.WithMany()
            //.HasForeignKey(a => a.UserId);
            //}
            //public DbSet<ecommerce.Models.Address>? Address { get; set; }
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableDetailedErrors(); // This will enable detailed error messages
        }
    }
}