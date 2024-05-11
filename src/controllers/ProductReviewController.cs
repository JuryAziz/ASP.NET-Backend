using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/productreviews")]
public class ProductReviewController(AppDbContext appDbContext) : ControllerBase
{
    private readonly ProductReviewService _productReviewService = new (appDbContext);

    [HttpGet]
    public async Task<IActionResult> GetProductReviews([FromQuery] int page = 1, [FromQuery] int limit = 50)
    {
        List<ProductReview> productReviews = await _productReviewService.GetProductReviews();
        List<ProductReview> paginatedProductReviews = Paginate.Function(productReviews, page, limit);
        return Ok(new BaseResponseList<ProductReview>(paginatedProductReviews, true));
    }

    [HttpGet("{reviewId}")]
    public async Task<IActionResult> GetProductReviewById(string reviewId)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Review ID Format"));

        ProductReview? review = await _productReviewService.GetProductReviewById(reviewIdGuid);
        if (review is null) return NotFound();

        return Ok(new BaseResponse<ProductReview>(review, true));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductReview([FromBody] ProductReviewModel newReview)
    {
        ProductReview? newProductReview = await _productReviewService.CreateProductReview(newReview);
        return Ok(new BaseResponse<ProductReview>(newProductReview, true));
    }

    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateProductReview(string reviewId, [FromBody] ProductReviewModel updatedReview)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Review ID Format"));

        ProductReview? reviewToBeUpdated = await _productReviewService.GetProductReviewById(reviewIdGuid);
        if (reviewToBeUpdated is null) return NotFound();
        ProductReview? UpdatedReview = await _productReviewService.UpdateProductReview(reviewIdGuid, updatedReview);

        return Ok(new BaseResponse<ProductReview>(UpdatedReview, true));
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteProductReview(string reviewId)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Review ID Format"));
       
        ProductReview? reviewToBeDeleted = await _productReviewService.GetProductReviewById(reviewIdGuid);
        if (reviewToBeDeleted is null || !await _productReviewService.DeleteProductReview(reviewIdGuid)) return NotFound();
      
        return Ok(new BaseResponse<ProductReview>(reviewToBeDeleted, true));
    }
}