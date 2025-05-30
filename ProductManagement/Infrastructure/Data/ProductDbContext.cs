using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Models;
using ProductManagement.Domain.ValueObjects;

namespace ProductManagement.Infrastructure.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                    .HasConversion(id => id.Value,
                        value => new ProductId(value));

                entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Type).IsRequired();
                entity.Property(p => p.Quantity).IsRequired();
            });
        }
    }
}