using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;

using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.Application.Services;

public class PaymentMethodService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<PaymentMethod>> GetPaymentMethods()
    {
        return await _appDbContext.PaymentMethods
            .ToListAsync();

    }

    public async Task<PaymentMethod?> GetPaymentMethodById(Guid paymentMethodId)
    {
        return await Task.FromResult(
            await _appDbContext.PaymentMethods
            .FirstOrDefaultAsync(pm => pm.PaymentMethodId == paymentMethodId));
    }

    public async Task<PaymentMethod?> CreatePaymentMethod(PaymentMethodModel newPaymentMethod)
    {
        var paymentMethod = new PaymentMethod
        {
            UserId = newPaymentMethod.UserId,
            Type = newPaymentMethod.Type,
            CardNumber = newPaymentMethod.CardNumber,
            CardHolderName = newPaymentMethod.CardHolderName,
            CardExpirationDate = newPaymentMethod.CardExpirationDate,
            CardCCV = newPaymentMethod.CardCCV,
            IsDefault = newPaymentMethod.IsDefault,
            CreatedAt = DateTime.UtcNow,
        };

        await _appDbContext.PaymentMethods.AddAsync(paymentMethod);
        await _appDbContext.SaveChangesAsync();
        return await Task.FromResult(paymentMethod);
    }

    public async Task<PaymentMethod?> UpdatePaymentMethod(Guid paymentMethodId, PaymentMethodModel updatedPaymentMethod)
    {
        var paymentMethodToUpdate = await GetPaymentMethodById(paymentMethodId);
        if (paymentMethodToUpdate is not null)
        {
            paymentMethodToUpdate.Type = updatedPaymentMethod.Type;
            paymentMethodToUpdate.CardNumber = updatedPaymentMethod.CardNumber;
            paymentMethodToUpdate.CardHolderName = updatedPaymentMethod.CardHolderName;
            paymentMethodToUpdate.CardExpirationDate = updatedPaymentMethod.CardExpirationDate;
            paymentMethodToUpdate.CardCCV = updatedPaymentMethod.CardCCV;
            paymentMethodToUpdate.IsDefault = updatedPaymentMethod.IsDefault;

            await _appDbContext.SaveChangesAsync();
        };

        return await Task.FromResult(paymentMethodToUpdate);
    }

    public async Task<bool> DeletePaymentMethod(Guid paymentMethodId)
    {
        var paymentMethodToDelete = await GetPaymentMethodById(paymentMethodId);
        if (paymentMethodToDelete is null) return await Task.FromResult(false);

        _appDbContext.PaymentMethods.Remove(paymentMethodToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}