using Microsoft.EntityFrameworkCore;

namespace SqlIntegrationTesting;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Order>()
            .HasIndex(u => u.ExternalId)
            .IsUnique();
    }
}