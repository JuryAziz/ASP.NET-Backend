using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        //###########################
        //      TableBuilder
        //###########################

        builder.ToTable("Order");
        builder.HasKey(o => o.OrderId);

        builder
            .Property(u => u.OrderId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(u => u.UserId)
            .IsRequired();

        builder
            .Property(ad => ad.AddressId)
            .IsRequired();

        builder
            .Property(pm => pm.PaymentMethodId)
            .IsRequired();

        builder
            .Property(t => t.TransactionId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(s => s.ShipmentId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(o => o.Status)
            .IsRequired();

        builder
            .Property(o => o.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        //###########################
        //      TableRelations
        //###########################

        builder
            .HasMany(o => o.Items)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

    }
}
