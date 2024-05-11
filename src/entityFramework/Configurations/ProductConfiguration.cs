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

        builder
            .HasMany(p => p.Reviews)
            .WithOne(pr => pr.Product)
            .HasForeignKey(pr => pr.ProductId);
    }
}
