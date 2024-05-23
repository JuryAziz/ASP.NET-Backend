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
        builder.HasKey(p => p.ProductId);
        
        builder
            .Property(c => c.ProductId) 
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder
            .Property(c => c.Name)
            .IsRequired();

        builder
            .Property(c => c.Price)
            .IsRequired();

        builder
            .Property(c => c.Stock)
            .IsRequired();

        //###########################
        //      TableRelations
        //###########################

        // builder
        //     .HasMany(p => p.Categories)
        //     .WithMany(c => c.Products);

    }
}
