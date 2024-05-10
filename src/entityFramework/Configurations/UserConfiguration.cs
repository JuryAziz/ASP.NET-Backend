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

        builder.ToTable("User");
        builder.HasKey(u => u.UserId);

        builder
        .Property(u => u.UserId)
        .IsRequired()
        .ValueGeneratedOnAdd();

        builder
        .HasIndex(u => u.Email)
        .IsUnique();

        builder
        .HasIndex(u => u.PhoneNumber)
        .IsUnique();

        builder
        .Property(u => u.FirstName)
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
