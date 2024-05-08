using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Store.entityFramework;
using Store.EntityFramework.Entities;
using Store.Models;

// done for now 

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

    public async Task<IEnumerable<PaymentMethod>> GetUserPaymentMethods(Guid userId)
    {
        // return await Task.FromResult(_paymentMethods.FindAll(pm => pm.UserId == userId));

        var userPaymentMethods = await _appDbContext.PaymentMethods
            .Where(pm => pm.UserId == userId)
            .ToListAsync();

        return userPaymentMethods;

    }

    public async Task<PaymentMethod?> GetPaymentMethodById(Guid paymentMethodId)
    {
        // return await Task.FromResult(_paymentMethods.FirstOrDefault(pm => pm.PaymentMethodId == paymentMethodId));

        var paymentMethods = await _appDbContext.PaymentMethods
        .FirstOrDefaultAsync(pm => pm.PaymentMethodId == paymentMethodId);

        return paymentMethods;

    }

    public async Task<PaymentMethod?> CreatePaymentMethod(PaymentMethodModel newPaymentMethod)
    {
        // newPaymentMethod.PaymentMethodId = Guid.NewGuid();
        // newPaymentMethod.CreatedAt = DateTime.Now;
        // _paymentMethods.Add(newPaymentMethod);
        // return await Task.FromResult(newPaymentMethod);


        // newPaymentMethod.PaymentMethodId = Guid.NewGuid();
        // newPaymentMethod.CreatedAt = DateTime.Now;
        // _appDbContext.Add(newPaymentMethod);
        // return await Task.FromResult(newPaymentMethod);

        var paymentMethod = new PaymentMethod
        {
            PaymentMethodId = Guid.NewGuid(),
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

        // paymentId
        // UserId
        // type
        // CardNumber
        // cardName
        // card expire 
        // ccv
        // isDefault
        // CreatedAt 
        // ?? orders array ??

    }

    public async Task<PaymentMethod?> UpdatePaymentMethod(Guid paymentMethodId, PaymentMethodModel updatedPaymentMethod)
    {
        // var paymentMethodToUpdate = _paymentMethods.FirstOrDefault(pm => pm.PaymentMethodId == paymentMethodId);
        // if (paymentMethodToUpdate != null)
        // {
        //     paymentMethodToUpdate.Type = updatedPaymentMethod.Type;
        //     paymentMethodToUpdate.CardNumber = updatedPaymentMethod.CardNumber;
        //     paymentMethodToUpdate.CardHolderName = updatedPaymentMethod.CardHolderName;
        //     paymentMethodToUpdate.CardExpirationDate = updatedPaymentMethod.CardExpirationDate;
        //     paymentMethodToUpdate.CardCCV = updatedPaymentMethod.CardCCV;
        //     paymentMethodToUpdate.IsDefault = updatedPaymentMethod.IsDefault;
        // };

        // return await Task.FromResult(paymentMethodToUpdate);

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
        // var paymentMethodToDelete = _paymentMethods.FirstOrDefault(pm => pm.PaymentMethodId == paymentMethodId);
        // if (paymentMethodToDelete != null)
        // {
        //     _paymentMethods.Remove(paymentMethodToDelete);
        //     return await Task.FromResult(true);
        // };
        // return await Task.FromResult(false);

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