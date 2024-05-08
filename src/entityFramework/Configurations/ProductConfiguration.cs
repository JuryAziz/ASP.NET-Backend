using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;


namespace Store.EntityFramework.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {

        builder.ToTable("product");
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



        /*builder
        .HasMany(product => product.CategoryList)
        .WithMany(category => category.ProductList)
        .UsingEntity(
            "product_category",
            l => l.HasOne(typeof(Product)).WithMany().HasForeignKey("product_id"),
            r => r.HasOne(typeof(Category)).WithMany().HasForeignKey("category_id"));*/




        /**
         builder
            .HasMany(product => product.UserEntityListByReview)
            .WithMany(user => user.ProductEntityListByReview)
            .UsingEntity<ProductReviewEntity>(
                 l => l.HasOne(productReview => productReview.UserEntity).WithMany(user => user.ProductReviewEntityList).HasForeignKey(productReview => productReview.UserId),

                r => r.HasOne(productReview => productReview.ProductEntity).WithMany(product => product.ProductReviewEntityList).HasForeignKey(productReview => productReview.ProductId)

                );
        
        */

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
