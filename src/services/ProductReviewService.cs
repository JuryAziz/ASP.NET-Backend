using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.Application.Services;

public class ProductReviewService
{
    private readonly AppDbContext _appDbContext;
    public ProductReviewService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<ProductReview>> GetProductReviews()
    {
        return await _appDbContext.ProductReviews.ToListAsync();
    }

    public async Task<ProductReview?> GetProductReviewById(Guid reviewId)
    {
        return await _appDbContext.ProductReviews.FirstOrDefaultAsync(review => review.ReviewId == reviewId);
    }

    public async Task<ProductReview?> AddProductReview(ProductReviewModel newReview)
    {
        var productReview = new ProductReview
        {
            ReviewId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Rating = newReview.Rating,
            Title = newReview.Title,
            Description = newReview.Description,
            CreatedAt = DateTime.UtcNow
        };
        await _appDbContext.AddAsync(productReview);
        await _appDbContext.SaveChangesAsync();
        return productReview;
    }

    public async Task<ProductReview?> UpdateProductReview(Guid reviewId, ProductReviewModel updatedReview)
    {
        var reviewToBeUpdated = await _appDbContext.ProductReviews.FindAsync(reviewId);
        if (reviewToBeUpdated is null) return null;

        reviewToBeUpdated.Rating = updatedReview.Rating;
        reviewToBeUpdated.Title = updatedReview.Title;
        reviewToBeUpdated.Description = updatedReview.Description;
        reviewToBeUpdated.CreatedAt = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();
        return reviewToBeUpdated;
    }

    public async Task<ProductReview?> DeleteProductReview(Guid reviewId)
    {
        var reviewToBeDeleted = await GetProductReviewById(reviewId);
        if (reviewToBeDeleted is null) return null;
        _appDbContext.ProductReviews.Remove(reviewToBeDeleted!);
        await _appDbContext.SaveChangesAsync();
        return reviewToBeDeleted;
    }
}
