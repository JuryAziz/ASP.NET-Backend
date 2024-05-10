using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;
using Store.Models;

namespace Store.Application.Services;

public class OrderItemService
{
    private readonly AppDbContext _appDbContext;

    public OrderItemService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItems(int page, int limit)
    {
        var orderItems = _appDbContext.OrderItem.ToList();
        var pagedOrderItem = orderItems[((page - 1) * limit)..(_appDbContext.OrderItem.Count() > (page * limit) ? page * limit : _appDbContext.OrderItem.Count())];
        return await Task.FromResult(pagedOrderItem.AsEnumerable());

        // return await Task.FromResult(_OrderItems[((page - 1) * limit)..(_OrderItems.Count > (page * limit) ? page * limit : _OrderItems.Count)].AsEnumerable());
    }

    // public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrder(Guid orderId)
    // {
    //     return await Task.FromResult(
    //         (await _appDbContext.OrderItem.ToListAsync())
    //         .FindAll(oi => oi.OrderId == orderId));
    // }

    public async Task<OrderItem?> GetOrderItemById(Guid orderItemId)
    {
         return await Task.FromResult(
            await _appDbContext.OrderItem
            .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId));
    }

    public async Task<OrderItem?> CreateOrderItems(OrderItemModel newOrderItem)
    {
        var orderItem = new OrderItem
        {
            OrderItemId = Guid.NewGuid(),
            // OrderId = newOrderItem.OrderId,
            // ProductId = newOrderItem.ProductId,
            Price = newOrderItem.Price,
            Quantity = newOrderItem.Quantity,
            CreatedAt = DateTime.UtcNow,
        };

        await _appDbContext.OrderItem.AddAsync(orderItem);
        await _appDbContext.SaveChangesAsync();
        return await Task.FromResult(orderItem);
    }

    public async Task<OrderItem?> UpdateOrderItems(Guid orderItemId, OrderItemModel updatedOrderItem)
    {
        var orderItemToUpdate = await _appDbContext.OrderItem
        .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
        if (orderItemToUpdate is not null)
        {
            orderItemToUpdate.Price = updatedOrderItem.Price;
            orderItemToUpdate.Quantity = updatedOrderItem.Quantity;
            await _appDbContext.SaveChangesAsync();
        };
        return await Task.FromResult(orderItemToUpdate);
    }

    public async Task<bool> DeleteOrderItem(Guid orderItemId)
    {
        var orderItemToDelete = await _appDbContext.OrderItem
        .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
        if (orderItemToDelete is not null)
        {
            _appDbContext.OrderItem.Remove(orderItemToDelete);
            await _appDbContext.SaveChangesAsync();
            return await Task.FromResult(true);
        };
        return await Task.FromResult(false);
    }
}
