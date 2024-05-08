using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.entityFramework;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;

[ApiController]
[Route("/api/paymentmethods")]
public class PaymentMethodsController : Controller
{
    private readonly PaymentMethodService _paymentMethodService;

    public PaymentMethodsController(AppDbContext appDbContext)
    {
        // _paymentMethodService = paymentMethodService;
        _paymentMethodService = new PaymentMethodService(appDbContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaymentMethods([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {

            if (limit > 20) limit = 20;
            IEnumerable<PaymentMethod>? paymentMethods = await _paymentMethodService.GetPaymentMethods(page, limit);
            var response = new BaseResponseList<PaymentMethod>(paymentMethods, true);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetPaymentMethods' page {page} limit {limit}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{paymentMethodId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetPaymentMethodById(string paymentMethodId)
    {
        try
        {
            if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
            PaymentMethod? foundPaymentMethod = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
            if (foundPaymentMethod is null) return NotFound(); ;
            return Ok(new BaseResponse<PaymentMethod>(foundPaymentMethod, true));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetPaymentMethodById'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaymentMethod(PaymentMethodModel newPaymentMethod)
    {
        try
        {
            PaymentMethod? createdPaymentMethod = await _paymentMethodService.CreatePaymentMethod(newPaymentMethod);
            return CreatedAtAction(nameof(GetPaymentMethodById), new { createdPaymentMethod?.PaymentMethodId }, createdPaymentMethod);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'CreatePaymentMethod'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{paymentMethodId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdatePaymentMethod(string paymentMethodId, PaymentMethodModel rawUpdatedPaymentMethod)
    {
        try
        {
            if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
            PaymentMethod? paymentMethodToUpdate = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
            if (paymentMethodToUpdate == null) return NotFound(); ;
            await _paymentMethodService.UpdatePaymentMethod(paymentMethodIdGuid, rawUpdatedPaymentMethod);
            return Ok(new BaseResponse<PaymentMethod>(paymentMethodToUpdate, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'UpdatePaymentMethod'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{paymentMethodId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeletePaymentMethod(string paymentMethodId)
    {
        try
        {
            if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
            PaymentMethod? paymentMethodToDelete = await _paymentMethodService.GetPaymentMethodById(paymentMethodIdGuid);
            if (paymentMethodToDelete == null || !await _paymentMethodService.DeletePaymentMethod(paymentMethodIdGuid)) return NotFound();
            return Ok(new BaseResponse<PaymentMethod>(paymentMethodToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'DeletePaymentMethod'");
            return StatusCode(500, ex.Message);
        }
    }
}