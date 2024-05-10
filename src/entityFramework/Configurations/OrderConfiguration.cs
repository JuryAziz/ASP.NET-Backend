using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
// using Store.Models;

namespace Store.EntityFramework.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        ///#####################
        //TableBuilder
        //######################
        builder.ToTable("Order");

        builder.HasKey(o => o.OrderId);

        builder.Property(u => u.OrderId)
        .ValueGeneratedOnAdd();

        // builder.Property(u => u.UserId)
        // .IsRequired();

        // builder.Property(ad => ad.AddressId)
        // .IsRequired();

        builder.Property(pm => pm.PaymentMethodId)
        .IsRequired();

        builder.Property(t => t.TransactionId)
        .IsRequired()
        .ValueGeneratedOnAdd();

        builder.Property(s => s.ShipmentId)
        .IsRequired()
        .ValueGeneratedOnAdd();

        builder.Property(o => o.Status)
        .IsRequired();

        builder.Property(o => o.CreatedAt)
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
