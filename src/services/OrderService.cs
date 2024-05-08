using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.entityFramework;
using Store.Models;

namespace Store.Application.Services;

public class OrderService
{
    // private readonly static List<OrderModel> _Orders = [
    //         new() {
    //             OrderId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8100"),
    //             UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
    //             AddressId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8f43"),
    //             PaymentMethodId = Guid.Parse("3fcb9f36-4cb1-451e-9562-4c4d915a2c24"),
    //             TransactionId = Guid.Empty, // Not set yet
    //             ShipmentId = Guid.Empty,
    //             Status = OrderModel.OrderStatus.Pending,
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new() {
    //             OrderId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8101"),
    //             UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
    //             AddressId = Guid.Parse("d0bc5b89-2303-49c6-bc99-1fa7ef18c313"),
    //             PaymentMethodId = Guid.Parse("dfc68e6e-3025-4fef-946a-9ac1385234fa"),
    //             TransactionId = Guid.Empty, // Not set yet
    //             ShipmentId = Guid.Empty,
    //             Status = OrderModel.OrderStatus.Pending,
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new() {
    //             OrderId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8102"),
    //             UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
    //             AddressId = Guid.Parse("046a584e-d497-487b-aaad-f4e3cfb5b6f0"),
    //             PaymentMethodId = Guid.Parse("f22248fb-d5b2-4829-ae96-75ab59f3ff22"),
    //             TransactionId = Guid.Empty, // Not set yet
    //             ShipmentId = Guid.Empty,
    //             Status = OrderModel.OrderStatus.Pending,
    //             CreatedAt = DateTime.UtcNow
    //         }
    //     ];

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
        // return await Task.FromResult(_Orders.FirstOrDefault(order => order.OrderId == orderId));
         return await Task.FromResult(
            await _appDbContext.Order
            .FirstOrDefaultAsync(o => o.OrderId == orderId));
    }

    public async Task<Order?> CreateOrders(OrderModel newOrder)
    {
        // newOrder.OrderId = Guid.NewGuid();
        // newOrder.CreatedAt = DateTime.Now;
        // _Orders.Add(newOrder);
        // return await Task.FromResult(newOrder);

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
