using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Helpers;
using Store.Models;

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
            var reviews = await _productReviewService.GetProductReviews();
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetProductReviews'");
            return StatusCode(500, new BaseResponse<ProductReviewModel> { Message = ex.Message });
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

            return BadRequest(e);
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
            return BadRequest("ops");
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
            return BadRequest("deletion failed");
        }
    }
}
