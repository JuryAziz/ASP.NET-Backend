using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;


namespace Store.EntityFramework.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        //###########################
        //      TableBuilder
        //###########################

        builder.ToTable("Product");
        builder.Property(p => p.ProductId);

        //###########################
        //      TableBuilder
        //###########################

        // builder.ToTable("product");
        builder.HasKey(c => c.ProductId);

        builder.Property(c => c.ProductId).HasColumnName("product_id").IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
        builder.Property(c => c.Name).HasColumnName("name").IsRequired();
        builder.Property(c => c.Description).HasColumnName("description");
        builder.Property(c => c.Price).HasColumnName("price").IsRequired();
        builder.Property(c => c.Stock).HasColumnName("total_quantity").IsRequired().HasDefaultValue(0).ValueGeneratedOnAdd();

        builder.HasIndex(u => u.ProductId).IsUnique();

        // Product Has many ProductCategory
        builder
            .HasMany(product => product.ProductCategoryList)
            .WithOne(productCategory => productCategory.Product)
            .HasForeignKey(productCategory => productCategory.ProductId)
            .IsRequired();

        // many to many using bridge table ProductCategory between Category and Product
        builder
            .HasMany(product => product.CategoryList)
            .WithMany(category => category.ProductList)
            .UsingEntity<ProductCategory>(
                l => l.HasOne(productCategory => productCategory.Category).WithMany(category => category.ProductCategoryList).HasForeignKey(productCategory => productCategory.CategoryId),
                r => r.HasOne(productCategory => productCategory.Product).WithMany(product => product.ProductCategoryList).HasForeignKey(productCategory => productCategory.ProductId)
        );

    }
}
