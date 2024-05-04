using Store.Models;

namespace Store.Application.Services;
public class PaymentMethodService
{

    private readonly static List<PaymentMethodModel> _paymentMethods = [
        new(){
                PaymentMethodId = Guid.Parse("3fcb9f36-4cb1-451e-9562-4c4d915a2c24"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                Type = "jcb",
                CardNumber = 3529302130289644,
                CardHolderName = "Jury Alharbi",
                CardExpirationDate = new DateTime(2024-04),
                CardCCV = 657,
                IsDefault = false
            },
            new(){
                PaymentMethodId = Guid.Parse("dfc68e6e-3025-4fef-946a-9ac1385234fa"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                Type = "jcb",
                CardNumber = 3585865165102396,
                CardHolderName = "Jury Alharbi",
                CardExpirationDate = new DateTime(2024-04),
                CardCCV = 553,
                IsDefault = true
            },
            new(){
                PaymentMethodId = Guid.Parse("f22248fb-d5b2-4829-ae96-75ab59f3ff22"),
                UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
                Type = "jcb",
                CardNumber = 3543801716110878,
                CardHolderName = "Ammar Banigetah",
                CardExpirationDate = new DateTime(2024-03),
                CardCCV = 441,
                IsDefault = false
            },
            new(){
                PaymentMethodId = Guid.Parse("1b6c4ef2-41cb-4744-a6bf-2e4b4d18badf"),
                UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
                Type = "instapayment",
                CardNumber = 6393565916946282,
                CardHolderName = "Ammar Banigetah",
                CardExpirationDate = new DateTime(2023-08),
                CardCCV = 943,
                IsDefault = true
            },
            new(){
                PaymentMethodId = Guid.Parse("0e518afe-1a3f-411d-91be-6bb8a1c5b42d"),
                UserId = Guid.Parse("49e4b1ef-483c-48f7-ad70-01226369542d"),
                Type = "americanexpress",
                CardNumber = 374283350858262,
                CardHolderName = "Abdulhadi Saeed",
                CardExpirationDate = new DateTime(2024-03),
                CardCCV = 548,
                IsDefault = false
            },
            new(){
                PaymentMethodId = Guid.Parse("b96becaa-49e4-40c3-b877-81e82b03e526"),
                UserId = Guid.Parse("49e4b1ef-483c-48f7-ad70-01226369542d"),
                Type = "jcb",
                CardNumber = 3576135472181744,
                CardHolderName = "Abdulhadi Saeed",
                CardExpirationDate = new DateTime(2024-01),
                CardCCV = 569,
                IsDefault = true
            },
            new(){
                PaymentMethodId = Guid.Parse("43cc0259-0c07-44c6-a4f5-0201fcb2d55d"),
                UserId = Guid.Parse("bc0ab7fd-d613-4aa6-bbb2-99725746e90e"),
                Type = "jcb",
                CardNumber = 3575960080480332,
                CardHolderName = "Ayman Bahatheg",
                CardExpirationDate = new DateTime(2024-03),
                CardCCV = 757,
                IsDefault = false
            },
            new(){
                PaymentMethodId = Guid.Parse("7529482a-e57c-47d3-9dc3-57c4ad9e28bf"),
                UserId = Guid.Parse("bc0ab7fd-d613-4aa6-bbb2-99725746e90e"),
                Type = "americanexpress",
                CardNumber = 378896920824435,
                CardHolderName = "Ayman Bahatheg",
                CardExpirationDate = new DateTime(2023-09),
                CardCCV = 940,
                IsDefault = true
            },
            new(){
                PaymentMethodId = Guid.Parse("b657711e-2b09-404c-9124-7451d235cbb0"),
                UserId = Guid.Parse("a080f42b-db4b-4246-8f20-ec52cc35667e"),
                Type = "jcb",
                CardNumber = 3537046077866316,
                CardHolderName = "Ibrahim ALHarbi",
                CardExpirationDate = new DateTime(2023-12),
                CardCCV = 971,
                IsDefault = false
            },
            new(){
                PaymentMethodId = Guid.Parse("d582c261-fa4d-4ba4-aadc-6fc828a2991c"),
                UserId = Guid.Parse("a080f42b-db4b-4246-8f20-ec52cc35667e"),
                Type = "jcb",
                CardNumber = 3559720432775767,
                CardHolderName = "Ibrahim ALHarbi",
                CardExpirationDate = new DateTime(2023-05),
                CardCCV = 450,
                IsDefault = true
            }
    ];

    public async Task<IEnumerable<PaymentMethodModel>> GetPaymentMethods(int page, int limit)
    {
        return await Task.FromResult(_paymentMethods[((page - 1) * limit)..(_paymentMethods.Count > (page * limit) ? page * limit : _paymentMethods.Count)].AsEnumerable());
    }

    public async Task<IEnumerable<PaymentMethodModel>> GetUserPaymentMethods(Guid userId)
    {
        return await Task.FromResult(_paymentMethods.FindAll(pm => pm.UserId == userId));
    }

    public async Task<PaymentMethodModel?> GetPaymentMethodById(Guid paymentMethodId)
    {
        return await Task.FromResult(_paymentMethods.FirstOrDefault(pm => pm.PaymentMethodId == paymentMethodId));
    }

    public async Task<PaymentMethodModel?> CreatePaymentMethod(PaymentMethodModel newPaymentMethod)
    {
        newPaymentMethod.PaymentMethodId = Guid.NewGuid();
        newPaymentMethod.CreatedAt = DateTime.Now;
        _paymentMethods.Add(newPaymentMethod);
        return await Task.FromResult(newPaymentMethod);
    }

    public async Task<PaymentMethodModel?> UpdatePaymentMethod(Guid paymentMethodId, PaymentMethodModel updatedPaymentMethod)
    {
        var paymentMethodToUpdate = _paymentMethods.FirstOrDefault(pm => pm.PaymentMethodId == paymentMethodId);
        if (paymentMethodToUpdate != null)
        {
            paymentMethodToUpdate.Type = updatedPaymentMethod.Type;
            paymentMethodToUpdate.CardNumber = updatedPaymentMethod.CardNumber;
            paymentMethodToUpdate.CardHolderName = updatedPaymentMethod.CardHolderName;
            paymentMethodToUpdate.CardExpirationDate = updatedPaymentMethod.CardExpirationDate;
            paymentMethodToUpdate.CardCCV = updatedPaymentMethod.CardCCV;
            paymentMethodToUpdate.IsDefault = updatedPaymentMethod.IsDefault;
        };

        return await Task.FromResult(paymentMethodToUpdate);
    }

    public async Task<bool> DeletePaymentMethod(Guid paymentMethodId)
    {
        var paymentMethodToDelete = _paymentMethods.FirstOrDefault(pm => pm.PaymentMethodId == paymentMethodId);
        if (paymentMethodToDelete != null)
        {
            _paymentMethods.Remove(paymentMethodToDelete);
            return await Task.FromResult(true);
        };
        return await Task.FromResult(false);
    }
}
