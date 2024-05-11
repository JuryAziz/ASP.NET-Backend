using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Dtos;
using Store.EntityFramework;

using Store.EntityFramework.Entities;

namespace Store.Application.Services;

public class PaymentMethodService(AppDbContext appDbContext, IMapper mapper)
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<PaymentMethodDto>> GetPaymentMethods()
    {
        return (await _appDbContext.PaymentMethods
            .ToListAsync()).Select(_mapper.Map<PaymentMethodDto>);
    }

    public async Task<PaymentMethodDto?> GetPaymentMethodById(Guid paymentMethodId)
    {
        return await Task.FromResult((await GetPaymentMethods()).FirstOrDefault(pm => pm.PaymentMethodId == paymentMethodId));
    }

    public async Task<PaymentMethodDto?> CreatePaymentMethod(CreatePaymentMethodDto newPaymentMethod)
    {
        PaymentMethod paymentMethod = new () {
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

        return await Task.FromResult(_mapper.Map<PaymentMethodDto>(paymentMethod));
    }

    public async Task<PaymentMethodDto?> UpdatePaymentMethod(Guid paymentMethodId, UpdatePaymentMethodDto updatedPaymentMethod)
    {
        PaymentMethodDto? paymentMethodToUpdate = await GetPaymentMethodById(paymentMethodId);
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

        return await Task.FromResult(_mapper.Map<PaymentMethodDto>(paymentMethodToUpdate));
    }

    public async Task<DeletePaymentMethodDto?> DeletePaymentMethod(Guid paymentMethodId)
    {
        PaymentMethod? paymentMethodToDelete = await _appDbContext.PaymentMethods.FirstOrDefaultAsync(paymentMethod => paymentMethod.PaymentMethodId == paymentMethodId);
        if (paymentMethodToDelete is null) return null;

        _appDbContext.PaymentMethods.Remove(paymentMethodToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(_mapper.Map<DeletePaymentMethodDto>(paymentMethodToDelete));
    }
}