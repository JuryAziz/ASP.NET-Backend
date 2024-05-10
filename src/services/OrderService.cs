using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;

using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.Application.Services;
public class OrderService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<Order>> GetOrders()
    {
        return await _appDbContext.Orders
            .Include(o => o.User)
            .Include(o => o.Address)
            .Include(o => o.PaymentMethod)
            .Include(o => o.Items)
            .ToListAsync();
    }


    public async Task<Order?> GetOrderById(Guid orderId)
    {
        return await Task.FromResult((await GetOrders()).FirstOrDefault(o => o.OrderId == orderId));
    }

    public async Task<Order?> CreateOrders(OrderModel newOrder)
    {
        var order = new Order
        {
            UserId = newOrder.UserId,
            AddressId = newOrder.AddressId,
            PaymentMethodId = newOrder.PaymentMethodId,
            TransactionId = newOrder.TransactionId,
            ShipmentId = newOrder.ShipmentId,
            Status = newOrder.Status,
            CreatedAt = DateTime.UtcNow,
        };

        await _appDbContext.Orders.AddAsync(order);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(order);
    }

    public async Task<Order?> UpdateOrders(Guid orderId, OrderModel updatedOrder)
    {
        var orderToUpdate = await GetOrderById(orderId);
        if (orderToUpdate is not null)
        {
            orderToUpdate.Status = updatedOrder.Status;

            await _appDbContext.SaveChangesAsync();
        };
        return await Task.FromResult(orderToUpdate);
    }

    public async Task<bool> DeleteOrder(Guid orderId)
    {
        var orderToDelete = await GetOrderById(orderId);
        if (orderToDelete is null) return await Task.FromResult(false);

        _appDbContext.Orders.Remove(orderToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}
