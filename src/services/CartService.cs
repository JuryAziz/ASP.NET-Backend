using Store.EntityFramework;
using Store.Models;

namespace Store.Application.Services;

public class CartService
{
    private readonly static List<CartModel> _carts = [
    new() {
                CartId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8f43"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                Items = []
            },
            new() {
                CartId = Guid.Parse("d0bc5b89-2303-49c6-bc99-1fa7ef18c313"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                Items = []
            },
            new() {
                CartId = Guid.Parse("046a584e-d497-487b-aaad-f4e3cfb5b6f0"),
                UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
                Items = []
            },
            new() {
                CartId = Guid.Parse("4f8659f7-d16c-4207-b6ca-5892fc333a64"),
                UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
                Items = []
            },
            new() {
                CartId = Guid.Parse("e4bc5d0b-56a5-4815-aa57-8e2d4e4eca38"),
                UserId = Guid.Parse("49e4b1ef-483c-48f7-ad70-01226369542d"),
                Items = []
            },
        ];

    public async Task<IEnumerable<CartModel>> GetCarts(int page, int limit)
    {
        return await Task.FromResult(_carts[((page - 1) * limit)..(_carts.Count > (page * limit) ? page * limit : _carts.Count)].AsEnumerable());
    }

    public async Task<CartModel?> GetUserCart(Guid userId)
    {
        return await Task.FromResult(_carts.Find(cart => cart.UserId == userId));
    }


    public async Task<CartModel?> GetCartById(Guid cartId)
    {
        return await Task.FromResult(_carts.FirstOrDefault(cart => cart.CartId == cartId));
    }

    public async Task<CartModel?> CreateCart(CartModel newCart)
    {
        newCart.CartId = Guid.NewGuid();
        _carts.Add(newCart);
        return await Task.FromResult(newCart);
    }

    public async Task<CartModel?> UpdateCart(Guid cartId, CartModel updatedCart)
    {
        var cartToUpdate = _carts.FirstOrDefault(cart => cart.CartId == cartId);
        if (cartToUpdate is not null)
        {
            cartToUpdate.Items = updatedCart.Items;
        };
        return await Task.FromResult(cartToUpdate);
    }

    public async Task<bool> DeleteCart(Guid cartId)
    {
        var cartToDelete = _carts.FirstOrDefault(cart => cart.CartId == cartId);
        if (cartToDelete is not null)
        {
            _carts.Remove(cartToDelete);
            return await Task.FromResult(true);
        };
        return await Task.FromResult(false);
    }
}
