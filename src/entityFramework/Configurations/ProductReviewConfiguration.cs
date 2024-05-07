using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.EntityFramework.Entities;

namespace Store.EntityFramework.Configurations;

public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        ///#####################
        //Table Builder
        //######################

        builder.ToTable("ProductReviews");

        builder.HasKey(pr => pr.ReviewId);

        builder.Property(pr => pr.ReviewId)
        .IsRequired();

        builder.Property(pr => pr.Title)
        .IsRequired();

        builder.
        Property(pr => pr.Rating)
        .IsRequired();

        builder.Property(pr => pr.Description)
        .HasMaxLength(500)
        .HasAnnotation("MinLength", 10);
    }
}
