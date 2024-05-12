using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Store.Helpers.Enums;
using AutoMapper;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/productreviews")]
public class ProductReviewController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
{
    private readonly ProductReviewService _productReviewService = new (appDbContext);
    private readonly AuthSerivce _authService = new (appDbContext, mapper);

    #pragma warning disable CS8604 // Possible null reference argument.

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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateProductReview([FromBody] ProductReviewModel newReview)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        ProductReview? newProductReview = await _productReviewService.CreateProductReview(newReview);
        return Ok(new BaseResponse<ProductReview>(newProductReview, true));
    }

    [Authorize]
    [HttpPut("{reviewId}")]
    public async Task<IActionResult> UpdateProductReview(string reviewId, [FromBody] ProductReviewModel updatedReview)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Review ID Format"));

        ProductReview? reviewToBeUpdated = await _productReviewService.GetProductReviewById(reviewIdGuid);
        if (reviewToBeUpdated is null) return NotFound();
        ProductReview? UpdatedReview = await _productReviewService.UpdateProductReview(reviewIdGuid, updatedReview);

        return Ok(new BaseResponse<ProductReview>(UpdatedReview, true));
    }

    [Authorize]
    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> DeleteProductReview(string reviewId)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(reviewId, out Guid reviewIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Review ID Format"));
       
        ProductReview? reviewToBeDeleted = await _productReviewService.GetProductReviewById(reviewIdGuid);
        if (reviewToBeDeleted is null || !await _productReviewService.DeleteProductReview(reviewIdGuid)) return NotFound();
      
        return Ok(new BaseResponse<ProductReview>(reviewToBeDeleted, true));
    }
}