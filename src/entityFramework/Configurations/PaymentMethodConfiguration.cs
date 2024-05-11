using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;
public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        //###########################
        //      TableBuilder
        //###########################

        builder.ToTable("PaymentMethod");
        builder.HasKey(pm => pm.PaymentMethodId);

        // primary key is required and auto genrate
        builder
            .Property(pm => pm.PaymentMethodId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(pm => pm.UserId)
            .IsRequired();

        builder
            .Property(pm => pm.Type)
            .IsRequired()
            .HasMaxLength(20);

        builder
            .HasIndex(pm => pm.CardNumber)
            .IsUnique();

        builder
            .Property(pm => pm.CardNumber)
            .IsRequired();

        builder
            .Property(pm => pm.CardHolderName)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(pm => pm.CardExpirationDate)
            .IsRequired();

        builder
            .Property(pm => pm.CardCCV)
            .IsRequired()
            .HasColumnType("decimal(3,0)");

        builder
            .Property(pm => pm.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

      
        //###########################
        //      TableBuilder
        //###########################

    }
}
