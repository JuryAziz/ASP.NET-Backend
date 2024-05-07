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
            Console.WriteLine($"An error occured while 'GetProductReviews'");
            return StatusCode(500, new BaseResponse<ProductReview> { Message = ex.Message });
        }
    }

    [HttpGet("{reviewId::regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetProductReviewById(string reviewId)
    {
        if (Guid.TryParse(reviewId, out Guid reviewIdGuid))
        {
            var review = await _productReviewService.GetProductReviewById(reviewIdGuid);
            if (review is not null)
            {
                return Ok(new BaseResponse<ProductReview>(review, true));
            }
            else
            {
                return NotFound("Review not found, Nothing to show");
            }
        }
        else
        {
            return BadRequest("Something went wrong");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddProductReview([FromBody] ProductReviewModel newReview)
    {
        if (newReview is null)
        {
            return BadRequest("add a review");
        }
        try
        {
            await _productReviewService.AddProductReview(newReview);
            return Ok("review added successfully");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProductReview(string reviewId, [FromBody] ProductReviewModel updatedReview)
    {
        if (Guid.TryParse(reviewId, out Guid reviewIdGuid))
        {
            return Ok(await _productReviewService.UpdateProductReview(reviewIdGuid, updatedReview));
        }
        else
        {
            return BadRequest("Could not update the review");
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProductReview(string reviewId)
    {
        if (Guid.TryParse(reviewId, out Guid reviewIdGuid))
        {
            return Ok(await _productReviewService.DeleteProductReview(reviewIdGuid));
        }
        else
        {
            return BadRequest("Could not delete the review");
        }
    }
}
