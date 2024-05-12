using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Models;
using AutoMapper;
using Store.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Store.Helpers.Enums;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/paymentmethods")]
public class PaymentMethodsController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
{
    private readonly PaymentMethodService _paymentMethodService = new(appDbContext, mapper);
    private readonly AuthSerivce _authService = new (appDbContext, mapper);

    #pragma warning disable CS8604 // Possible null reference argument.

    [HttpGet]
    public async Task<IActionResult> GetPaymentMethods([FromQuery] int page = 1, [FromQuery] int limit = 50)
    {
        IEnumerable<PaymentMethodDto> paymentMethods = await _paymentMethodService.GetPaymentMethods();
        IEnumerable<PaymentMethodDto> paginatedPaymentMethods = Paginate.Function(paymentMethods.ToList(), page, limit);
        return Ok(new BaseResponseList<PaymentMethodDto>(paginatedPaymentMethods, true));
    }

    [HttpGet("{paymentMethodId}")]
    public async Task<IActionResult> GetPaymentMethodById(string paymentMethodId)
    {
        if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
       
        PaymentMethodDto? foundPaymentMethod = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
        if (foundPaymentMethod is null) return NotFound();

        return Ok(new BaseResponse<PaymentMethodDto>(foundPaymentMethod, true));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePaymentMethod(CreatePaymentMethodDto newPaymentMethod)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        PaymentMethodDto? createdPaymentMethod = await _paymentMethodService.CreatePaymentMethod(newPaymentMethod);
        return CreatedAtAction(nameof(GetPaymentMethodById), new { createdPaymentMethod?.PaymentMethodId }, createdPaymentMethod);
    }

    [Authorize]
    [HttpPut("{paymentMethodId}")]
    public async Task<IActionResult> UpdatePaymentMethod(string paymentMethodId, UpdatePaymentMethodDto rawUpdatedPaymentMethod)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
      
        PaymentMethodDto? paymentMethodToUpdate = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
        if (paymentMethodToUpdate is null) return NotFound(); ;
        PaymentMethodDto? updatedPaymentMethod = await _paymentMethodService.UpdatePaymentMethod(paymentMethodIdGuid, rawUpdatedPaymentMethod);

        return Ok(new BaseResponse<PaymentMethodDto>(updatedPaymentMethod, true));
    }

    [Authorize]
    [HttpDelete("{paymentMethodId}")]
    public async Task<IActionResult> DeletePaymentMethod(string paymentMethodId)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));

        PaymentMethodDto? paymentMethodToDelete = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
        if (paymentMethodToDelete is null) return NotFound();
        DeletePaymentMethodDto? deletedPaymentMethod = await _paymentMethodService.DeletePaymentMethod(paymentMethodIdGuid);
        if(deletedPaymentMethod is null) return NotFound();

        return Ok(new BaseResponse<DeletePaymentMethodDto>(deletedPaymentMethod, true));
    }
}