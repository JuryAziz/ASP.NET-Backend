using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;

using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.Application.Services;

public class CartItemService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<CartItem>> GetCartItems()
    {
        return await _appDbContext.CartItems
            // .Include(c => c.Product)  //uncomment o me
            .ToListAsync();
    }

    public async Task<CartItem?> GetCartItemById(Guid cartItemId)
    {
        return await Task.FromResult((await GetCartItems()).FirstOrDefault(ci => ci.CartItemId == cartItemId));
    }

    public async Task<CartItem?> CreateCartItems(CartItemModel newCartItem)
    {
        var cartItem = new CartItem
        {
            CartId = newCartItem.CartId,
            ProductId = newCartItem.ProductId,
            Quantity = newCartItem.Quantity,
        };

        await _appDbContext.CartItems.AddAsync(cartItem);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(cartItem);
    }

    public async Task<CartItem?> UpdateCartItems(Guid cartItemId, CartItemModel updatedCartItem)
    {
        var cartItemToUpdate = await GetCartItemById(cartItemId);
        if (cartItemToUpdate is null) return null;

        cartItemToUpdate.ProductId = updatedCartItem.ProductId;
        cartItemToUpdate.Quantity = updatedCartItem.Quantity;

        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(cartItemToUpdate);
    }

    public async Task<bool> DeleteCartItem(Guid cartItemId)
    {
        var cartItemToDelete = await GetCartItemById(cartItemId);
        if (cartItemToDelete is null) return await Task.FromResult(false);

        _appDbContext.CartItems.Remove(cartItemToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}
