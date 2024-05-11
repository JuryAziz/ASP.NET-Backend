using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;
public class CartProductConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        //###########################
        //      TableBuilder
        //###########################

        builder.ToTable("CartItem");
        builder.Property(ci => ci.CartItemId);

        builder
            .Property(ci => ci.CartItemId)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder
            .Property(ci => ci.Quantity)
            .IsRequired();

        builder
            .Property(ci => ci.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
        //###########################
        //      TableBuilder
        //###########################

    }
}
