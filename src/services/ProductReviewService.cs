using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;

using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.Application.Services;

public class ProductReviewService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<ProductReview>> GetProductReviews()
    {
        return await _appDbContext.ProductReviews
            .Include(pr => pr.User)
            .Include(pr => pr.OrderItem)
            .Include(pr => pr.Product)
            .ToListAsync();    
    }

    public async Task<ProductReview?> GetProductReviewById(Guid reviewId)
    {
        return await Task.FromResult((await GetProductReviews()).FirstOrDefault(pr => pr.ReviewId == reviewId));
    }

    public async Task<ProductReview?> CreateProductReview(ProductReviewModel newReview)
    {
        var productReview = new ProductReview
        {
            UserId = newReview.UserId,
            OrderItemId = newReview.OrderItemId,
            ProductId = newReview.ProductId,
            Rating = newReview.Rating,
            Title = newReview.Title,
            Description = newReview.Description,
            CreatedAt = DateTime.UtcNow
        };
        await _appDbContext.AddAsync(productReview);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(productReview);
    }

    public async Task<ProductReview?> UpdateProductReview(Guid reviewId, ProductReviewModel updatedReview)
    {
        var reviewToBeUpdated = await GetProductReviewById(reviewId);
        if (reviewToBeUpdated is null) return null;

        reviewToBeUpdated.Rating = updatedReview.Rating;
        reviewToBeUpdated.Title = updatedReview.Title;
        reviewToBeUpdated.Description = updatedReview.Description;
        reviewToBeUpdated.CreatedAt = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(reviewToBeUpdated);
    }

    public async Task<bool> DeleteProductReview(Guid reviewId)
    {
        var reviewToBeDeleted = await GetProductReviewById(reviewId);
        if (reviewToBeDeleted is null) return false;

        _appDbContext.ProductReviews.Remove(reviewToBeDeleted!);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}
