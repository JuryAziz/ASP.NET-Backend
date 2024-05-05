using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;
// using Store.Models;

namespace Store.entityFramework.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        ///#####################
        //TableBuilder
        //######################

        builder.ToTable("OrderItem");

        builder.HasKey(oi => oi.OrderItemId);

        builder.Property(oi => oi.OrderItemId)
        .IsRequired()
        .ValueGeneratedOnAdd();

        builder.Property(o => o.OrderId)
        .IsRequired();

        builder.Property(p => p.ProductId)
        .IsRequired();

        builder.Property(oi => oi.Price)
        .IsRequired();

        builder.Property(oi => oi.Quantity)
        .IsRequired();

        builder.Property(oi => oi.CreatedAt)
        .HasDefaultValueSql("CURRENT_TIMESTAMP");


        //builder.ToTable("TableName");
        //builder.Property(c => c.ClomenName).

        ///#####################
        //Table Relation
        ///#####################

        //builder.HasMany(c => c.ClomenName)
        // .WithOne(o => o.ClomenName)
        // .HasForeignKey(o => o.ClomenName);
    }
}
