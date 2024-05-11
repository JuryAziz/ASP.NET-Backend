using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //###########################
        //      TableBuilder
        //###########################

        builder.ToTable("User");
        builder.HasKey(u => u.UserId);

        builder
            .Property(u => u.UserId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .HasIndex(u => u.Email)
            .IsUnique();

        builder
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        builder
            .Property(u => u.FirstName)
            .IsRequired();

        builder
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        //###########################
        //      TableBuilder
        //###########################

        // Address Relation 1:N
        builder
            .HasMany(u => u.Addresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

        // Payment Methods Relation 1:N
        builder
            .HasMany(u => u.PaymentMethods)
            .WithOne(pm => pm.User)
            .HasForeignKey(pm => pm.UserId);

        // Orders Relation 1:N
        builder
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);

        // Shopping Lists Relation 1:N
        // builder
        //     .HasOne(u => u.ShoppingLists)
        //     .WithOne(c => c.User)
        //     .HasForeignKey(c => c.UserId);

        // Cart Relation 1:1
        builder
            .HasOne(u => u.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserId);

        // Product Reviews Relation 1:N
        builder
            .HasMany(u => u.ProductReviews)
            .WithOne(pr => pr.User)
            .HasForeignKey(pr => pr.UserId);
    }
}
