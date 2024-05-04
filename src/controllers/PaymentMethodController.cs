using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/paymentmethods")]
public class PaymentMethodsController(PaymentMethodService paymentMethodService) : ControllerBase
{
    private readonly PaymentMethodService _paymentMethods = paymentMethodService;

    [HttpGet]
    public async Task<IActionResult> GetPaymentMethods([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {
            if (limit > 20) limit = 20;
            IEnumerable<PaymentMethodModel>? paymentMethods = await _paymentMethods.GetPaymentMethods(page, limit);
            return Ok(new BaseResponseList<PaymentMethodModel>(paymentMethods, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetPaymentMethods' page {page} limit {limit}");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpGet("{paymentMethodId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetPaymentMethodById(string paymentMethodId)
    {
        try
        {
            if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
            PaymentMethodModel? foundPaymentMethod = await _paymentMethods.GetPaymentMethodById(paymentMethodIdGuid);
            if (foundPaymentMethod == null) return NotFound();;
            return Ok(new BaseResponse<PaymentMethodModel>(foundPaymentMethod, true));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetUserById'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePaymentMethod(PaymentMethodModel newPaymentMethod)
    {
        try
        {
            PaymentMethodModel? createdPaymentMethod = await _paymentMethods.CreatePaymentMethod(newPaymentMethod);
            return CreatedAtAction(nameof(GetPaymentMethodById), new { createdPaymentMethod?.PaymentMethodId }, createdPaymentMethod);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'CreateUser'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpPut("{paymentMethodId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdatePaymentMethod(string paymentMethodId, PaymentMethodModel rawUpdatedPaymentMethod)
    {
        try
        {
            if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
            PaymentMethodModel? paymentMethodToUpdate = await _paymentMethods.GetPaymentMethodById(paymentMethodIdGuid);
            if (paymentMethodToUpdate == null) return NotFound();;
            await _paymentMethods.UpdatePaymentMethod(paymentMethodIdGuid, rawUpdatedPaymentMethod);
            return Ok(new BaseResponse<PaymentMethodModel>(paymentMethodToUpdate, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'UpdateUser'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpDelete("{paymentMethodId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeletePaymentMethod(string paymentMethodId)
    {
        try
        {
            if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid PaymentMethod ID Format"));
            PaymentMethodModel? paymentMethodToDelete = await _paymentMethods.GetPaymentMethodById(paymentMethodIdGuid);
            if (paymentMethodToDelete == null || !await _paymentMethods.DeletePaymentMethod(paymentMethodIdGuid)) return NotFound();
            return Ok(new BaseResponse<PaymentMethodModel>(paymentMethodToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'DeleteUser'");
            return StatusCode(500, ex.Message );
        }
    }

}