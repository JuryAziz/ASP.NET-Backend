using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        //###########################
        //      TableBuilder
        //###########################

        builder.ToTable("Category");

        builder.HasKey(c => c.CategoryId);
        builder
            .Property(c => c.CategoryId) 
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder
            .Property(c => c.Name)
            .IsRequired();

        builder
            .HasIndex(c => c.Name)
            .IsUnique();


        //###########################
        //      TableRelations
        //###########################
        
        // builder
        //     .HasMany(c => c.Products)
        //     .WithMany(p => p.Categories);
    }
}
