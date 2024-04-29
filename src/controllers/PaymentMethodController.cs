using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.PaymentMethods;
using Store.Models.PaymentMethod;

namespace Store.API.Controllers.PaymentMethods
{
    [ApiController]
    [Route("/api/paymentmethods")]
    public class PaymentMethodsController(PaymentMethodService paymentMethodService) : ControllerBase
    {
        private readonly PaymentMethodService _paymentMethods = paymentMethodService;

        [HttpGet]
        public async Task<IActionResult> GetPaymentMethods()
        {
            return Ok(await _paymentMethods.GetPaymentMethods());
        }

        [HttpGet("{paymentMethodId:guid}")]
        public async Task<IActionResult> GetPaymentMethodById(Guid paymentMethodId)
        {
            var foundPaymentMethod = await _paymentMethods.GetPaymentMethodById(paymentMethodId);
            if (foundPaymentMethod == null) return NotFound();
            return Ok(foundPaymentMethod);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod(PaymentMethod newPaymentMethod)
        {
            var createdPaymentMethod = await _paymentMethods.CreatePaymentMethod(newPaymentMethod);
            return CreatedAtAction(nameof(GetPaymentMethodById), new { createdPaymentMethod?.PaymentMethodId }, createdPaymentMethod);
        }

        [HttpPut("{paymentMethodId:guid}")]
        public async Task<IActionResult> UpdatePaymentMethod(Guid paymentMethodId, PaymentMethod paymentMethod)
        {
            var paymentMethodToUpdate = await _paymentMethods.GetPaymentMethodById(paymentMethodId);
            if (paymentMethodToUpdate == null) return NotFound();
            await _paymentMethods.UpdatePaymentMethod(paymentMethodId, paymentMethod);
            return Ok(paymentMethodToUpdate);
        }

        [HttpDelete("{paymentMethodId:guid}")]
        public async Task<IActionResult> DeletePaymentMethod(Guid paymentMethodId)
        {
            var paymentMethodToDelete = await _paymentMethods.GetPaymentMethodById(paymentMethodId);
            if (paymentMethodToDelete == null) return NotFound();
            if (!await _paymentMethods.DeletePaymentMethod(paymentMethodId)) return NotFound();
            return Ok($"Payment Method with the ID of ({paymentMethodId}) was deleted!");
        }
    }
}