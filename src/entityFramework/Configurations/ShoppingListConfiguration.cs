using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;

public class ShoppingListConfiguration : IEntityTypeConfiguration<ShoppingList>
{
    public void Configure(EntityTypeBuilder<ShoppingList> builder)
    {
        //###########################
        //      TableBuilder
        //###########################

        builder.ToTable("ShoppingList");
        builder.Property(sl => sl.ShoppingListId);

        builder
            .Property(sl => sl.ShoppingListId)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(sl => sl.Name)
            .IsRequired();

        //###########################
        //      TableBuilder
        //###########################

        builder
            .HasMany(sl => sl.Items);
    }
}
