using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.EntityFramework.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryModel>
{
    public void Configure(EntityTypeBuilder<CategoryModel> builder)
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
