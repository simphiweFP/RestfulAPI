using Microsoft.EntityFrameworkCore;
using ShippingApi.Models;

namespace ShippingApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(address => address.Street).HasMaxLength(200).IsRequired();
                entity.Property(address => address.City).HasMaxLength(100).IsRequired();
                entity.Property(address => address.ZipCode).HasMaxLength(20).IsRequired();
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.Property(driver => driver.Name).HasMaxLength(150).IsRequired();
                entity.Property(driver => driver.Email).HasMaxLength(254).IsRequired();
                entity.Property(driver => driver.Team).HasMaxLength(100).IsRequired();
                entity.HasIndex(driver => driver.DriverNumber).IsUnique();
                entity.HasOne(driver => driver.Address)
                    .WithMany()
                    .HasForeignKey("AddressId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(order => order.TotalAmount).HasPrecision(18, 2);
                entity.HasMany(order => order.Items)
                    .WithOne()
                    .HasForeignKey("OrderId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(item => item.Name).HasMaxLength(200).IsRequired();
                entity.Property(item => item.Price).HasPrecision(18, 2);
            });
        }
    }
}
