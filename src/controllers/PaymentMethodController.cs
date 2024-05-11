using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Models;
using AutoMapper;
using Store.Dtos;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/paymentmethods")]
public class PaymentMethodsController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
{
    private readonly PaymentMethodService _paymentMethodService = new(appDbContext, mapper);

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

    [HttpPost]
    public async Task<IActionResult> CreatePaymentMethod(CreatePaymentMethodDto newPaymentMethod)
    {
        PaymentMethodDto? createdPaymentMethod = await _paymentMethodService.CreatePaymentMethod(newPaymentMethod);
        return CreatedAtAction(nameof(GetPaymentMethodById), new { createdPaymentMethod?.PaymentMethodId }, createdPaymentMethod);
    }

    [HttpPut("{paymentMethodId}")]
    public async Task<IActionResult> UpdatePaymentMethod(string paymentMethodId, UpdatePaymentMethodDto rawUpdatedPaymentMethod)
    {
        if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
      
        PaymentMethodDto? paymentMethodToUpdate = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
        if (paymentMethodToUpdate is null) return NotFound(); ;
        PaymentMethodDto? updatedPaymentMethod = await _paymentMethodService.UpdatePaymentMethod(paymentMethodIdGuid, rawUpdatedPaymentMethod);

        return Ok(new BaseResponse<PaymentMethodDto>(updatedPaymentMethod, true));
    }

    [HttpDelete("{paymentMethodId}")]
    public async Task<IActionResult> DeletePaymentMethod(string paymentMethodId)
    {
        if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));

        PaymentMethodDto? paymentMethodToDelete = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
        if (paymentMethodToDelete is null) return NotFound();
        DeletePaymentMethodDto? deletedPaymentMethod = await _paymentMethodService.DeletePaymentMethod(paymentMethodIdGuid);
        if(deletedPaymentMethod is null) return NotFound();

        return Ok(new BaseResponse<DeletePaymentMethodDto>(deletedPaymentMethod, true));
    }
}