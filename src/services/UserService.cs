using Store.Models;

namespace Store.Application.Services;
public class UserService
{
    private readonly static List<UserModel> _users = [
        new(){UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"), Email = "JuriAbdulaziz.Alsubhi@integrify.io", PhoneNumber = "0540000000", FirstName = "Jury", LastName = "ALHarbi", DateOfBirth = new DateTime(), Role = 0},
            new(){UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"), Email = "ammarsalim.banigetah@integrify.io", PhoneNumber = "0540000000", FirstName = "Ammar", LastName = "Banigetah", DateOfBirth = new DateTime(), Role = 0},
            new(){UserId = Guid.Parse("49e4b1ef-483c-48f7-ad70-01226369542d"), Email = "AbdulhadiSaeed.Basulaiman@integrify.io", PhoneNumber = "0540000000", FirstName = "Abdulhadi", LastName = "Saeed", DateOfBirth = new DateTime(), Role = 0},
            new(){UserId = Guid.Parse("bc0ab7fd-d613-4aa6-bbb2-99725746e90e"), Email = "AymanAli.Bahatheg@integrify.io", PhoneNumber = "0540000000", FirstName = "Ayman", LastName = "Bahatheg", DateOfBirth = new DateTime(), Role = 0},
            new(){UserId = Guid.Parse("a080f42b-db4b-4246-8f20-ec52cc35667e"), Email = "IbrahimBurayk.ALHarbi@integrify.io", PhoneNumber = "0540000000", FirstName = "Ibrahim", LastName = "ALHarbi", DateOfBirth = new DateTime(), Role = 0},
        ];

    public async Task<IEnumerable<UserModel>> GetUsers(int Page, int limit)
    {
        return await Task.FromResult(_users[((Page - 1) * limit)..(_users.Count > (Page * limit) ? Page * limit : _users.Count)].AsEnumerable());
    }

    public async Task<UserModel?> GetUserById(Guid userId)
    {
        return await Task.FromResult(_users.FirstOrDefault(user => user.UserId == userId));
    }

    public async Task<UserModel?> CreateUser(UserModel newUser)
    {
        newUser.UserId = Guid.NewGuid();
        newUser.CreatedAt = DateTime.Now;
        _users.Add(newUser);
        return await Task.FromResult(newUser);
    }

    public async Task<UserModel?> UpdateUser(Guid userId, UserModel updatedUser)
    {
        var userToUpdate = _users.FirstOrDefault(user => user.UserId == userId);
        if (userToUpdate != null)
        {
            userToUpdate.Email = updatedUser.Email;
            userToUpdate.PhoneNumber = updatedUser.PhoneNumber;
            userToUpdate.FirstName = updatedUser.FirstName;
            userToUpdate.LastName = updatedUser.LastName;
            userToUpdate.DateOfBirth = updatedUser.DateOfBirth;
            userToUpdate.Role = updatedUser.Role;
        };
        return await Task.FromResult(userToUpdate);
    }

    public async Task<bool> DeleteUser(Guid userId)
    {
        var userToDelete = _users.FirstOrDefault(user => user.UserId == userId);
        if (userToDelete != null)
        {
            _users.Remove(userToDelete);
            return await Task.FromResult(true);
        };
        return await Task.FromResult(false);
    }
}
