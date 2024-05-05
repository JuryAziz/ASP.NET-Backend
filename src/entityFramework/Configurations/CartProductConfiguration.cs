using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.EntityFramework.Configurations;

public class CartProductConfiguration : IEntityTypeConfiguration<CartProductModel>
{
    public void Configure(EntityTypeBuilder<CartProductModel> builder)
    {
        ///#####################
        //TableBuilder
        //######################

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
