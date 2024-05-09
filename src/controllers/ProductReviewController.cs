using Microsoft.AspNetCore.Mvc;
using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;

[ApiController]
[Route("/api/reviews")]
public class ProductReviewController : ControllerBase
{
    private readonly ProductReviewService _productReviewService;
    public ProductReviewController(ProductReviewService productReviewService)
    {
        _productReviewService = productReviewService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductReviews()
    {
        try
        {
            IEnumerable<ProductReview> reviews = await _productReviewService.GetProductReviews();
            return Ok(new BaseResponseList<ProductReview>(reviews, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured could not get the reviews");
            return StatusCode(500, new BaseResponse<ProductReview> { Message = ex.Message });
        }
    }

    [HttpGet("{reviewId::regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetProductReviewById(string reviewId)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Review ID Format"));
        var review = await _productReviewService.GetProductReviewById(reviewIdGuid);
        if (review is null) return NotFound("Review not found, Nothing to show");
        return Ok(new BaseResponse<ProductReview>(review, true));
    }

    [HttpPost]
    public async Task<IActionResult> AddProductReview([FromBody] ProductReviewModel newReview)
    {
        if (newReview is null) return BadRequest(new BaseResponse<object>(false, "Something went wrong, please add a review"));
        try
        {
            await _productReviewService.AddProductReview(newReview);
            return Ok(new BaseResponse<ProductReviewModel>(newReview, true));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new BaseResponse<ProductReview> { Message = ex.Message });

        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProductReview(string reviewId, [FromBody] ProductReviewModel updatedReview)
    {
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid)) return BadRequest(new BaseResponse<object>(false, "Could not update the review"));
        var reviewToBeUpdated = await _productReviewService.UpdateProductReview(reviewIdGuid, updatedReview);
        if (reviewToBeUpdated is null) return NotFound("review not found, could not update");
        return Ok(new BaseResponse<ProductReview>(reviewToBeUpdated, true));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProductReview(string reviewId)
    {
        if (reviewId is null) return BadRequest("please add an ID");
        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid)) return BadRequest(new BaseResponse<object>(false, "Something went wrong, could not delete, the review, make the sure the ID is valid"));
        var reviewToBeDeleted = await _productReviewService.DeleteProductReview(reviewIdGuid);
        if (reviewToBeDeleted is null) return NotFound("review not found, could not delete");
        return Ok(new BaseResponse<ProductReview>(reviewToBeDeleted, true));
    }
}
