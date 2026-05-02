using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuration can be added here
        modelBuilder.Entity<Category>().HasMany(c => c.Products).WithOne(p => p.Category).HasForeignKey(p => p.CategoryId);
    }
}
