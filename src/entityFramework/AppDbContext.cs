using Microsoft.EntityFrameworkCore;
using Store.EntityFramework.Entities;

namespace Store.entityFramework;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Address { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }

    // use Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        // modelBuilder.Entity<User>()
        // .HasKey(user => user.UserId);

        // modelBuilder.Entity<User>()
        // .Property(user => user.UserId)
        // .IsRequired()
        // .ValueGeneratedOnAdd();

        // modelBuilder.Entity<User>()
        // .Property(user => user.Name)
        // .IsRequired();

        // modelBuilder.Entity<User>()
        // .Property(user => user.Email)
        // .IsRequired();

        // // 1-1 relation
        // modelBuilder.Entity<User>()
        // .HasOne(user => user.Profile)
        // .WithOne(p => p.User)
        // .HasForeignKey<Profile>(p => p.UserId);

        // // 1-many
        // modelBuilder.Entity<User>()
        // .HasMany(user => user.Orders)
        // .WithOne(o => o.User)
        // .HasForeignKey(o => o.UserId);


        // modelBuilder.Entity<Profile>()
        // .HasKey(e => e.ProfileId);

        // modelBuilder.Entity<Order>()
        // .HasKey(e => e.OrderId);
    }
}
