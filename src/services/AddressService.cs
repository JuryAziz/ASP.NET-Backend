using Store.Models.Address;

namespace Store.Application.Services.Addresses
{
    public class AddressService
    {

        private readonly static List<Address> _addresses = [
            new() {
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

        public async Task<IEnumerable<Address>> GetAddresses()
        {
            return await Task.FromResult(_addresses.AsEnumerable());
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