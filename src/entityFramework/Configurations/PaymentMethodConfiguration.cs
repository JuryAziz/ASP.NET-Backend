using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;
// using Store.Models;

namespace Store.entityFramework.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        ///#####################
        //TableBuilder
        //######################
        builder.ToTable("PaymentMethod");

        builder.HasKey(pm => pm.PaymentMethodId);

        // primary key is required and auto genrate
        builder.Property(pm => pm.PaymentMethodId)
        .IsRequired()
        .ValueGeneratedOnAdd()
        .HasDefaultValueSql("gen_random_uuid()");

        // setting user id as required
        builder.Property(u => u.UserId)
        .IsRequired();

        builder.Property(pm => pm.Type)
        .IsRequired()
        .HasMaxLength(20);

        builder.Property(pm => pm.CardNumber)
        .IsRequired()
        .HasColumnType("decimal(16,0)");

        builder.Property(pm => pm.CardHolderName)
        .IsRequired()
        .HasMaxLength(50);

        builder.Property(pm => pm.CardExpirationDate)
        .IsRequired();

        builder.Property(pm => pm.CardCCV)
        .IsRequired()
        .HasColumnType("decimal(3,0)");

        builder.Property(pm => pm.CreatedAt)
        .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // one to many from PaymentMethod to Orders
        builder.HasMany(pm => pm.Orders)
        .WithOne(o => o.PaymentMethod)
        .HasForeignKey(o => o.PaymentMethodId); // Foreign key in Order

        // relation to Order

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
