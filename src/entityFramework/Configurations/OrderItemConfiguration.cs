using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        //###########################
        //      TableBuilder
        //###########################

        builder.ToTable("OrderItem");
        builder.HasKey(oi => oi.OrderItemId);

        builder
            .Property(oi => oi.OrderItemId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(oi => oi.OrderId)
            .IsRequired();

        builder
            .Property(oi => oi.ProductId)
            .IsRequired();

        builder
            .Property(oi => oi.Price)
            .IsRequired();

        builder
            .Property(oi => oi.Quantity)
            .IsRequired();

        builder
            .Property(oi => oi.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        //###########################
        //      TableBuilder
        //###########################

        // Address Relation 1:1
        builder
            .HasOne(oi => oi.ProductReview)
            .WithOne(pr => pr.OrderItem);

    }
}
