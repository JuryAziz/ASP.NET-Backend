using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;

using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.Application.Services;

public class OrderItemService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<OrderItem>> GetOrderItems()
    {
        return await _appDbContext.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .ToListAsync();
    }

    public async Task<OrderItem?> GetOrderItemById(Guid orderItemId)
    {
        return await Task.FromResult((await GetOrderItems()).FirstOrDefault(oi => oi.OrderItemId == orderItemId));
    }

    public async Task<OrderItem?> CreateOrderItems(OrderItemModel newOrderItem)
    {
        var orderItem = new OrderItem
        {
            OrderItemId = Guid.NewGuid(),
            OrderId = newOrderItem.OrderId,
            ProductId = newOrderItem.ProductId,
            Price = newOrderItem.Price,
            Quantity = newOrderItem.Quantity,
            CreatedAt = DateTime.UtcNow,
        };

        await _appDbContext.OrderItems.AddAsync(orderItem);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(orderItem);
    }

    public async Task<OrderItem?> UpdateOrderItems(Guid orderItemId, OrderItemModel updatedOrderItem)
    {
        var orderItemToUpdate = await GetOrderItemById(orderItemId);
        if (orderItemToUpdate is null) return null;

        orderItemToUpdate.Price = updatedOrderItem.Price;
        orderItemToUpdate.Quantity = updatedOrderItem.Quantity;
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(orderItemToUpdate);
    }

    public async Task<bool> DeleteOrderItem(Guid orderItemId)
    {
        var orderItemToDelete = await GetOrderItemById(orderItemId);
        if (orderItemToDelete is null) return await Task.FromResult(false);

        _appDbContext.OrderItems.Remove(orderItemToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}
