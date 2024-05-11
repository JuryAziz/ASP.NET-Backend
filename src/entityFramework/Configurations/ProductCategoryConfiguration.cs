
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;
public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("product_category");
        builder.HasKey(c => c.ProductCategoryId);
        builder.Property(c => c.ProductCategoryId).HasColumnName("product_category_id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(a => a.CategoryId).HasColumnName("category_id").IsRequired();
        builder.Property(a => a.ProductId).HasColumnName("product_id").IsRequired();
        builder.HasIndex(u => u.ProductCategoryId).IsUnique();


        // productCategory Has one product
        builder
            .HasOne(productCategory => productCategory.Product)
            .WithMany(product => product.ProductCategoryList)
            .HasForeignKey(productCategory => productCategory.ProductId)
            .IsRequired();

        // productCategory Has one category
        builder
            .HasOne(productCategory => productCategory.Category)
            .WithMany(category => category.ProductCategoryList)
            .HasForeignKey(productCategory => productCategory.CategoryId)
            .IsRequired();
    }
}