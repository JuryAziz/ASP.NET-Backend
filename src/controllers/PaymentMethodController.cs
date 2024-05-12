using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/paymentmethods")]
public class PaymentMethodsController(AppDbContext appDbContext) : ControllerBase
{
    private readonly PaymentMethodService _paymentMethodService = new(appDbContext);

    [HttpGet]
    public async Task<IActionResult> GetPaymentMethods([FromQuery] int page = 1, [FromQuery] int limit = 50)
    {
        List<PaymentMethod> paymentMethods = await _paymentMethodService.GetPaymentMethods();
        List<PaymentMethod> paginatedPaymentMethods = Paginate.Function(paymentMethods, page, limit);
        return Ok(new BaseResponseList<PaymentMethod>(paginatedPaymentMethods, true));
    }

    [HttpGet("{paymentMethodId}")]
    public async Task<IActionResult> GetPaymentMethodById(string paymentMethodId)
    {
        if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
    
        PaymentMethod? foundPaymentMethod = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
        if (foundPaymentMethod is null) return NotFound();

        return Ok(new BaseResponse<PaymentMethod>(foundPaymentMethod, true));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaymentMethod(PaymentMethodModel newPaymentMethod)
    {
        PaymentMethod? createdPaymentMethod = await _paymentMethodService.CreatePaymentMethod(newPaymentMethod);
        return CreatedAtAction(nameof(GetPaymentMethodById), new { createdPaymentMethod?.PaymentMethodId }, createdPaymentMethod);
    }

    [HttpPut("{paymentMethodId}")]
    public async Task<IActionResult> UpdatePaymentMethod(string paymentMethodId, PaymentMethodModel rawUpdatedPaymentMethod)
    {
        if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
      
        PaymentMethod? paymentMethodToUpdate = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
        if (paymentMethodToUpdate is null) return NotFound(); ;
        PaymentMethod? updatedPaymentMethod = await _paymentMethodService.UpdatePaymentMethod(paymentMethodIdGuid, rawUpdatedPaymentMethod);

        return Ok(new BaseResponse<PaymentMethod>(updatedPaymentMethod, true));
    }

    [HttpDelete("{paymentMethodId}")]
    public async Task<IActionResult> DeletePaymentMethod(string paymentMethodId)
    {
        if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));

        PaymentMethod? paymentMethodToDelete = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
        if (paymentMethodToDelete is null || !await _paymentMethodService.DeletePaymentMethod(paymentMethodIdGuid)) return NotFound();

        return Ok(new BaseResponse<PaymentMethod>(paymentMethodToDelete, true));
    }
}