using Store.Models;

namespace Store.Application.Services;
public class ShoppingListService
{

    private readonly static List<ShoppingListModel> _shoppingLists = [
            new(){
                ShoppingListId = Guid.Parse("3fcb9f36-4cb1-451e-9562-4c4d915a2c24"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = false,
                CreatedAt = new DateTime(2024-04),
            },
            new(){
                ShoppingListId = Guid.Parse("dfc68e6e-3025-4fef-946a-9ac1385234fa"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = true,
                CreatedAt = new DateTime(2024-04),
            },
            new(){
                ShoppingListId = Guid.Parse("f22248fb-d5b2-4829-ae96-75ab59f3ff22"),
                UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = false,
                CreatedAt = new DateTime(2024-03),
            },
            new(){
                ShoppingListId = Guid.Parse("1b6c4ef2-41cb-4744-a6bf-2e4b4d18badf"),
                UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = true,
                CreatedAt = new DateTime(2023-08),
            },
            new(){
                ShoppingListId = Guid.Parse("0e518afe-1a3f-411d-91be-6bb8a1c5b42d"),
                UserId = Guid.Parse("49e4b1ef-483c-48f7-ad70-01226369542d"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = false,
                CreatedAt = new DateTime(2024-03),
            },
            new(){
                ShoppingListId = Guid.Parse("b96becaa-49e4-40c3-b877-81e82b03e526"),
                UserId = Guid.Parse("49e4b1ef-483c-48f7-ad70-01226369542d"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = true,
                CreatedAt = new DateTime(2024-01),
            },
            new(){
                ShoppingListId = Guid.Parse("43cc0259-0c07-44c6-a4f5-0201fcb2d55d"),
                UserId = Guid.Parse("bc0ab7fd-d613-4aa6-bbb2-99725746e90e"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = false,
                CreatedAt = new DateTime(2024-03),
            },
            new(){
                ShoppingListId = Guid.Parse("7529482a-e57c-47d3-9dc3-57c4ad9e28bf"),
                UserId = Guid.Parse("bc0ab7fd-d613-4aa6-bbb2-99725746e90e"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = true,
                CreatedAt = new DateTime(2023-09),
            },
            new(){
                ShoppingListId = Guid.Parse("b657711e-2b09-404c-9124-7451d235cbb0"),
                UserId = Guid.Parse("a080f42b-db4b-4246-8f20-ec52cc35667e"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = false,
                CreatedAt = new DateTime(2023-12),
            },
            new(){
                ShoppingListId = Guid.Parse("d582c261-fa4d-4ba4-aadc-6fc828a2991c"),
                UserId = Guid.Parse("a080f42b-db4b-4246-8f20-ec52cc35667e"),
                Name = "Some list name",
                Description = "Some list description",
                Items = [],
                IsPublic = true,
                CreatedAt = new DateTime(2023-05),
            }
    ];

    public async Task<IEnumerable<ShoppingListModel>> GetShoppingLists(int page, int limit)
    {
        return await Task.FromResult(_shoppingLists[((page - 1) * limit)..(_shoppingLists.Count > (page * limit) ? page * limit : _shoppingLists.Count)].AsEnumerable());
    }

    public async Task<ShoppingListModel?> GetShoppingListById(Guid shoppingListId)
    {
        return await Task.FromResult(_shoppingLists.FirstOrDefault(shoppingList => shoppingList.ShoppingListId == shoppingListId));
    }

    public async Task<ShoppingListModel?> CreateShoppingList(ShoppingListModel newShoppingList)
    {
        newShoppingList.UserId = Guid.NewGuid();
        newShoppingList.CreatedAt = DateTime.Now;
        _shoppingLists.Add(newShoppingList);

        return await Task.FromResult(newShoppingList);
    }

    public async Task<ShoppingListModel?> UpdateShoppingList(Guid shoppingListId, ShoppingListModel updatedShoppingList)
    {
        var shoppingListToUpdate = _shoppingLists.FirstOrDefault(shoppingList => shoppingList.ShoppingListId == shoppingListId);
        if (shoppingListToUpdate is not null)
        {
            shoppingListToUpdate.Name = updatedShoppingList.Name;
            shoppingListToUpdate.Description = updatedShoppingList.Description;
            shoppingListToUpdate.Items = updatedShoppingList.Items;
            shoppingListToUpdate.IsPublic = updatedShoppingList.IsPublic;
        };
        return await Task.FromResult(shoppingListToUpdate);
    }

    public async Task<bool> DeleteShoppingList(Guid shoppingListId)
    {
        var shoppingListToDelete = _shoppingLists.FirstOrDefault(shoppingList => shoppingList.ShoppingListId == shoppingListId);
        if (shoppingListToDelete is not null)
        {
            _shoppingLists.Remove(shoppingListToDelete);
            return await Task.FromResult(true);
        };
        return await Task.FromResult(false);
    }
}
