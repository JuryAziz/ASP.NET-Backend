using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.Application.Services;

public class PaymentMethodService
{
    private readonly AppDbContext _appDbContext;

    public PaymentMethodService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods(int page, int limit)
    {

        var paymentMethods = _appDbContext.PaymentMethods.ToList();
        var pagedPayment = paymentMethods[((page - 1) * limit)..(_appDbContext.PaymentMethods.Count() > (page * limit) ? page * limit : _appDbContext.PaymentMethods.Count())];
        return await Task.FromResult(pagedPayment.AsEnumerable());

    }

    // public async Task<IEnumerable<PaymentMethod>> GetUserPaymentMethods(Guid userId)
    // {
    //     return await Task.FromResult(
    //         (await _appDbContext.PaymentMethods.ToListAsync())
    //         .FindAll(pm => pm.UserId == userId)
    //         );

    // }

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
            PaymentMethodId = Guid.NewGuid(),
            // UserId = newPaymentMethod.UserId,
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
        var paymentMethodToUpdate = await _appDbContext.PaymentMethods
        .FirstOrDefaultAsync(pm => pm.PaymentMethodId == paymentMethodId);
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
        var paymentMethodToDelete = await _appDbContext.PaymentMethods
        .FirstOrDefaultAsync(pm => pm.PaymentMethodId == paymentMethodId);
        if (paymentMethodToDelete is not null)
        {
            _appDbContext.PaymentMethods.Remove(paymentMethodToDelete);
            await _appDbContext.SaveChangesAsync();
            return await Task.FromResult(true);
        };
        return await Task.FromResult(false);
    }
}