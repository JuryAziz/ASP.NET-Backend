using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;
using Store.Models;

namespace Store.Application.Services;

public class OrderService
{
    private readonly AppDbContext _appDbContext;

    public OrderService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<IEnumerable<Order>> GetOrders(int page, int limit)
    {
        var orders = _appDbContext.Order.ToList();
        var pagedOrder = orders[((page - 1) * limit)..(_appDbContext.Order.Count() > (page * limit) ? page * limit : _appDbContext.Order.Count())];
        return await Task.FromResult(pagedOrder.AsEnumerable());

        // return await Task.FromResult(_Orders[((page - 1) * limit)..(_Orders.Count > (page * limit) ? page * limit : _Orders.Count)].AsEnumerable());
    }

    // public async Task<IEnumerable<Order>> GetUserOrders(Guid userId)
    // {
    //     // return await Task.FromResult(_Orders.FindAll(order => order.UserId == userId));
    //     return await Task.FromResult(
    //         (await _appDbContext.Order.ToListAsync())
    //         .FindAll(o => o.UserId == userId)
    //     );
    // }

    public async Task<Order?> GetOrderById(Guid orderId)
    {
         return await Task.FromResult(
            await _appDbContext.Order
            .FirstOrDefaultAsync(o => o.OrderId == orderId));
    }

    public async Task<Order?> CreateOrders(OrderModel newOrder)
    {
        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            // UserId = newOrder.UserId,
            // AddressId = newOrder.AddressId,
            PaymentMethodId = newOrder.PaymentMethodId,
            TransactionId = newOrder.TransactionId,
            ShipmentId = newOrder.ShipmentId,
            Status = newOrder.Status,
            CreatedAt = DateTime.UtcNow,
        };

        await _appDbContext.Order.AddAsync(order);
        await _appDbContext.SaveChangesAsync();
        return await Task.FromResult(order);
    }

    public async Task<Order?> UpdateOrders(Guid orderId, OrderModel updatedOrder)
    {
        var orderToUpdate = await _appDbContext.Order
        .FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (orderToUpdate is not null)
        {
            orderToUpdate.Status = updatedOrder.Status;
            await _appDbContext.SaveChangesAsync();
        };
        return await Task.FromResult(orderToUpdate);
    }

    public async Task<bool> DeleteOrder(Guid orderId)
    {
        var orderToDelete = await _appDbContext.Order
        .FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (orderToDelete is not null)
        {
            _appDbContext.Order.Remove(orderToDelete);
            await _appDbContext.SaveChangesAsync();
            return await Task.FromResult(true);
        };
        return await Task.FromResult(false);
    }
}
