using Store.Models.Address;

namespace Store.Application.Services.Addresses
{
    public class AddressService
    {

        private readonly static List<Address> _addresses = [
            new() {
                AddressId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8f43"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                Country = "United States",
                State = "New Mexico",
                City = "Las Cruces",
                Address1 = "29 Rusk Point",
                Address2 = "Morrow",
                PostalCode = 88006,
                IsDefault = true
            },
            new() {
                AddressId = Guid.Parse("d0bc5b89-2303-49c6-bc99-1fa7ef18c313"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                Country = "United States",
                State = "Texas",
                City = "Austin",
                Address1 = "65 Sutteridge Plaza",
                Address2 = "Milwaukee",
                PostalCode = 78764,
                IsDefault = false
            },
            new() {
                AddressId = Guid.Parse("046a584e-d497-487b-aaad-f4e3cfb5b6f0"),
                UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
                Country = "United States",
                State = "Louisiana",
                City = "Baton Rouge",
                Address1 = "1 Pleasure Trail",
                Address2 = "Claremont",
                PostalCode = 70894,
                IsDefault = true
            },
            new() {
                AddressId = Guid.Parse("4f8659f7-d16c-4207-b6ca-5892fc333a64"),
                UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
                Country = "United States",
                State = "Alaska",
                City = "Fairbanks",
                Address1 = "6426 Fair Oaks Trail",
                Address2 = "Fremont",
                PostalCode = 99790,
                IsDefault = false
            },




            new() {
                AddressId = Guid.Parse("e4bc5d0b-56a5-4815-aa57-8e2d4e4eca38"),
                UserId = Guid.Parse("49e4b1ef-483c-48f7-ad70-01226369542d"),
                Country = "United States",
                State = "Texas",
                City = "Houston",
                Address1 = "73927 Schurz Circle",
                Address2 = "Vahlen",
                PostalCode = 77212,
                IsDefault = true
            },
            new() {
                AddressId = Guid.Parse("436e37b7-58d0-46bf-87e5-57ceb67a82f4"),
                UserId = Guid.Parse("49e4b1ef-483c-48f7-ad70-01226369542d"),
                Country = "United States",
                State = "Florida",
                City = "Jacksonville",
                Address1 = "3784 Charing Cross",
                Address2 = "Havey",
                PostalCode = 32204,
                IsDefault = false
            },
            new() {
                AddressId = Guid.Parse("1fb0f3f0-cdc2-4fbb-abcc-74e4609a02a4"),
                UserId = Guid.Parse("bc0ab7fd-d613-4aa6-bbb2-99725746e90e"),
                Country = "United States",
                State = "California",
                City = "Orange",
                Address1 = "4499 Monument Drive",
                Address2 = "Pepper Wood",
                PostalCode = 92668,
                IsDefault = true
            },
            new() {
                AddressId = Guid.Parse("58c90b6e-abaa-455f-bb17-ac44e5e818db"),
                UserId = Guid.Parse("bc0ab7fd-d613-4aa6-bbb2-99725746e90e"),
                Country = "United States",
                State = "Louisiana",
                City = "Lafayette",
                Address1 = "0916 Petterle Circle",
                Address2 = "Sutteridge",
                PostalCode = 70505,
                IsDefault = false
            },
            new() {
                AddressId = Guid.Parse("8be1c87b-6793-4d5b-8e93-2bb10d3a9a63"),
                UserId = Guid.Parse("a080f42b-db4b-4246-8f20-ec52cc35667e"),
                Country = "United States",
                State = "Oklahoma",
                City = "Tulsa",
                Address1 = "4 Paget Terrace",
                Address2 = "1st",
                PostalCode = 74108,
                IsDefault = true
            },
            new() {
                AddressId = Guid.Parse("bfe708e4-d019-405b-9aee-b57a8f40dfe3"),
                UserId = Guid.Parse("a080f42b-db4b-4246-8f20-ec52cc35667e"),
                Country = "United States",
                State = "Tennessee",
                City = "Memphis",
                Address1 = "164 Roth Point",
                Address2 = "Bashford",
                PostalCode = 38150,
                IsDefault = false
            },
        ];

        public async Task<IEnumerable<Address>> GetAddresses(int page, int limit)
        {
            return await Task.FromResult(_addresses[((page - 1) * limit)..(_addresses.Count > (page * limit) ? page * limit : _addresses.Count)].AsEnumerable());
        }

        public async Task<IEnumerable<Address>> GetUserAddresses(Guid userId)
        {
            return await Task.FromResult(_addresses.FindAll(address => address.UserId == userId));
        }

        public async Task<Address?> GetAddressById(Guid addressId)
        {
            return await Task.FromResult(_addresses.FirstOrDefault(address => address.AddressId == addressId));
        }

        public async Task<Address?> CreateAddress(Address newAddress)
        {
            newAddress.AddressId = Guid.NewGuid();
            newAddress.CreatedAt = DateTime.Now;
            _addresses.Add(newAddress);
            return await Task.FromResult(newAddress);
        }

        public async Task<Address?> UpdateAddress(Guid addressId, Address address)
        {
            var addressToUpdate = _addresses.FirstOrDefault(address => address.AddressId == addressId);
            if (addressToUpdate != null)
            {
                addressToUpdate.Country = address.Country;
                addressToUpdate.State = address.State;
                addressToUpdate.City = address.City;
                addressToUpdate.Address1 = address.Address1;
                addressToUpdate.Address2 = address.Address2;
                addressToUpdate.PostalCode = address.PostalCode;
                addressToUpdate.IsDefault = address.IsDefault;
            };

            return await Task.FromResult(addressToUpdate);
        }

        public async Task<bool> DeleteAddress(Guid addressId)
        {
            var addressToDelete = _addresses.FirstOrDefault(address => address.AddressId == addressId);
            if (addressToDelete != null)
            {
                _addresses.Remove(addressToDelete);
                return await Task.FromResult(true);
            };
            return await Task.FromResult(false);
        }
    }
}