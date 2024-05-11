using Microsoft.EntityFrameworkCore;

using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Models;

namespace Store.Application.Services;

public class ShoppingListService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<ShoppingList>> GetShoppingLists()
    {
        return await _appDbContext.ShoppingLists
            // .Include(sl => sl.Items)
            .ToListAsync();
    }

    public async Task<ShoppingList?> GetShoppingListById(Guid shoppingListId)
    {
        return await Task.FromResult((await GetShoppingLists()).FirstOrDefault(sl => sl.ShoppingListId == shoppingListId));

    }

    public async Task<ShoppingList?> CreateShoppingList(ShoppingListModel newShoppingList)
    {
        var shoppingList = new ShoppingList
        {
            UserId = newShoppingList.UserId,
            Name = newShoppingList.Name,
            Description = newShoppingList.Description,
            IsPublic = newShoppingList.IsPublic,
            Items = newShoppingList.Items,
        };

        await _appDbContext.ShoppingLists.AddAsync(shoppingList);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(shoppingList);
    }

    public async Task<ShoppingList?> UpdateShoppingList(Guid shoppingListId, ShoppingListModel updatedShoppingList)
    {
        var shoppingListToUpdate = await GetShoppingListById(shoppingListId);
        if (shoppingListToUpdate is null) return null;

        shoppingListToUpdate.Name = updatedShoppingList.Name;
        shoppingListToUpdate.Description = updatedShoppingList.Description;
        shoppingListToUpdate.IsPublic = updatedShoppingList.IsPublic;
        shoppingListToUpdate.Items = updatedShoppingList.Items;

        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(shoppingListToUpdate);
    }

    public async Task<bool> DeleteShoppingList(Guid shoppingListId)
    {
        var shoppingListToDelete = await GetShoppingListById(shoppingListId);
        if (shoppingListToDelete is null) return await Task.FromResult(false);

        _appDbContext.ShoppingLists.Remove(shoppingListToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}
