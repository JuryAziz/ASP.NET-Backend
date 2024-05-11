using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("category");

        builder.HasKey(c => c.CategoryId);
        builder.Property(c => c.CategoryId).HasColumnName("category_id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(c => c.Name).HasColumnName("name").IsRequired();
        builder.Property(c => c.Description).HasColumnName("description");

        builder.HasIndex(u => u.CategoryId).IsUnique();
        builder.HasIndex(u => u.Name).IsUnique();

        // category Has Many ProductCategory
        builder
        .HasMany(category => category.ProductCategoryList)
        .WithOne(productCategory => productCategory.Category)
        .HasForeignKey(productCategory => productCategory.CategoryId).IsRequired();

        // many to many using bridge table ProductCategory between Product and Category
        builder
            .HasMany(category => category.ProductList)
            .WithMany(product => product.CategoryList)
            .UsingEntity<ProductCategory>(
                l => l.HasOne(productCategory => productCategory.Product)
                    .WithMany(product => product.ProductCategoryList)
                    .HasForeignKey(productCategory => productCategory.ProductId).IsRequired(),
                r => r.HasOne(productCategory => productCategory.Category)
                    .WithMany(category => category.ProductCategoryList)
                    .HasForeignKey(productCategory => productCategory.CategoryId).IsRequired()
            );
    }
}
