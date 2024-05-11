using Microsoft.EntityFrameworkCore;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public AppDbContext(DbContextOptions options)
        : base(options) { }

    // public DbSet<User> Users { get; set; }
    // public DbSet<Profile> Profiles { get; set; }
    // public DbSet<Order> Orders { get; set; }    // public DbSet<Profile> Profiles { get; set; }
     public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }


    // use Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
