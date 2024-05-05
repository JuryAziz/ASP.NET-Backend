using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;
// using Store.Models;

namespace Store.EntityFramework.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> entityBuilder)
    {
        entityBuilder.ToTable("Cart");

        entityBuilder.HasKey(cart => cart.CartId);

        entityBuilder.Property(cart => cart.CartId)
        .IsRequired()
        .ValueGeneratedOnAdd();

        entityBuilder.HasOne(cart => cart.User)
                     .WithOne(user => user.Cart)
                     .HasForeignKey<User>(user => user.UserId);
    }
}
