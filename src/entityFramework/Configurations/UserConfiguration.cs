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

        builder.ToTable("Users");
        builder.HasKey(c => c.UserId);

        builder
        .Property(c => c.UserId)
        .IsRequired()
        .ValueGeneratedOnAdd();

        builder
        .HasIndex(c => c.Email)
        .IsUnique();

        builder
        .HasIndex(c => c.PhoneNumber)
        .IsUnique();

        builder
        .Property(c => c.FirstName)
        .IsRequired();

        //###########################
        //      TableBuilder
        //###########################

        // 1:N with Address
        builder
        .HasMany(u => u.Addresses)
        .WithOne(a => a.User)
        .HasForeignKey(a => a.UserId);
    }
}
