using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;
public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        //###########################
        //      TableBuilder
        //###########################

        builder.ToTable("Addresses");
        builder.HasKey(a => a.AddressId);

        builder
        .Property(a => a.AddressId)
        .IsRequired();

        builder
        .Property(a => a.Country)
        .IsRequired();

        builder
        .Property(a => a.State)
        .IsRequired();

        builder
        .Property(a => a.City)
        .IsRequired();

        builder
        .Property(a => a.PostalCode)
        .IsRequired();

        //###########################
        //      TableBuilder
        //###########################

    }
}
