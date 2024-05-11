using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework;

public class AppDbContext : DbContext
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
