using Microsoft.AspNetCore.Mvc;

using Store.Application.Services.PaymentMethods;

using Store.Models.PaymentMethod;

using Store.Helpers.Reponses;

namespace Store.API.Controllers.PaymentMethods
{
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
                var paymentMethods = await _paymentMethods.GetPaymentMethods(page, limit);
                return Ok(new SuccessResponse<IEnumerable<PaymentMethod>>() { Message = $"PaymentMethod fetched! page: {page}  limit: {limit}", Data = paymentMethods });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'GetPaymentMethods' page {page} limit {limit}");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpGet("{paymentMethodId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
        public async Task<IActionResult> GetPaymentMethodById(string paymentMethodId)
        {
            try
            {
                if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var foundPaymentMethod = await _paymentMethods.GetPaymentMethodById(paymentMethodIdGuid);
                if (foundPaymentMethod == null) return StatusCode(404, new SuccessResponse<string>() { Data = "Not Found" });
                return Ok(new SuccessResponse<PaymentMethod>() { Message = $"PaymentMethod found with Guid {foundPaymentMethod.PaymentMethodId}", Data = foundPaymentMethod });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'GetUserById'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod(PaymentMethod newPaymentMethod)
        {
            try
            {
                var createdPaymentMethod = await _paymentMethods.CreatePaymentMethod(newPaymentMethod);
                return CreatedAtAction(nameof(GetPaymentMethodById), new { createdPaymentMethod?.PaymentMethodId }, createdPaymentMethod);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'CreateUser'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpPut("{paymentMethodId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
        public async Task<IActionResult> UpdatePaymentMethod(string paymentMethodId, PaymentMethod paymentMethod)
        {
            try
            {
                if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var paymentMethodToUpdate = await _paymentMethods.GetPaymentMethodById(paymentMethodIdGuid);
                if (paymentMethodToUpdate == null) return StatusCode(404, new SuccessResponse<string>() { Message = "Not Found" });
                await _paymentMethods.UpdatePaymentMethod(paymentMethodIdGuid, paymentMethod);
                return Ok(new SuccessResponse<PaymentMethod>() { Message = $"User updated with guid of {paymentMethodToUpdate.PaymentMethodId}", Data = paymentMethodToUpdate });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'UpdateUser'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpDelete("{paymentMethodId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
        public async Task<IActionResult> DeletePaymentMethod(string paymentMethodId)
        {
            try
            {
                if (!Guid.TryParse(paymentMethodId, out Guid paymentMethodIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var paymentMethodToDelete = await _paymentMethods.GetPaymentMethodById(paymentMethodIdGuid);
                if (paymentMethodToDelete == null || !await _paymentMethods.DeletePaymentMethod(paymentMethodIdGuid)) return StatusCode(404, new SuccessResponse<string>() { Message = "Not Found" });
                return Ok(new SuccessResponse<PaymentMethod>() { Message = $"User deleted with the guid of {paymentMethodToDelete.PaymentMethodId}", Data = paymentMethodToDelete });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'DeleteUser'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

    }
}