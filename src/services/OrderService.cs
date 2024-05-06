using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Models;

namespace Store.Application.Services;

public class OrderService
{
    private readonly static List<OrderModel> _Orders = [
            new() {
                OrderId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8100"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                AddressId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8f43"),
                PaymentMethodId = Guid.Parse("3fcb9f36-4cb1-451e-9562-4c4d915a2c24"),
                TransactionId = Guid.Empty, // Not set yet
                ShipmentId = Guid.Empty,
                Status = OrderModel.OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            },
            new() {
                OrderId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8101"),
                UserId = Guid.Parse("180b142e-7026-4c25-b441-f6745da9d7f6"),
                AddressId = Guid.Parse("d0bc5b89-2303-49c6-bc99-1fa7ef18c313"),
                PaymentMethodId = Guid.Parse("dfc68e6e-3025-4fef-946a-9ac1385234fa"),
                TransactionId = Guid.Empty, // Not set yet
                ShipmentId = Guid.Empty,
                Status = OrderModel.OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            },
            new() {
                OrderId = Guid.Parse("dc3f7087-3e07-469a-a361-f0c7240c8102"),
                UserId = Guid.Parse("0ad0d823-4b20-4514-8e75-0fd6a908450c"),
                AddressId = Guid.Parse("046a584e-d497-487b-aaad-f4e3cfb5b6f0"),
                PaymentMethodId = Guid.Parse("f22248fb-d5b2-4829-ae96-75ab59f3ff22"),
                TransactionId = Guid.Empty, // Not set yet
                ShipmentId = Guid.Empty,
                Status = OrderModel.OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            }
        ];

    public async Task<IEnumerable<OrderModel>> GetOrders(int page, int limit)
    {
        return await Task.FromResult(_Orders[((page - 1) * limit)..(_Orders.Count > (page * limit) ? page * limit : _Orders.Count)].AsEnumerable());
    }

    public async Task<IEnumerable<OrderModel>> GetUserOrders(Guid userId)
    {
        return await Task.FromResult(_Orders.FindAll(order => order.UserId == userId));
    }

    public async Task<OrderModel?> GetOrderById(Guid orderId)
    {
        return await Task.FromResult(_Orders.FirstOrDefault(order => order.OrderId == orderId));
    }

    public async Task<OrderModel?> CreateOrders(OrderModel newOrder)
    {
        newOrder.OrderId = Guid.NewGuid();
        newOrder.CreatedAt = DateTime.Now;
        _Orders.Add(newOrder);
        return await Task.FromResult(newOrder);
    }

    public async Task<OrderModel?> UpdateOrders(Guid orderId, OrderModel updatedOrder)
    {
        var orderToUpdate = _Orders.FirstOrDefault(order => order.OrderId == orderId);
        if (orderToUpdate != null)
        {
            orderToUpdate.Status = updatedOrder.Status;
        };

        return await Task.FromResult(orderToUpdate);
    }

    public async Task<bool> DeleteOrder(Guid orderId)
    {
        var orderToDelete = _Orders.FirstOrDefault(order => order.OrderId == orderId);
        if (orderToDelete != null)
        {
            _Orders.Remove(orderToDelete);
            return await Task.FromResult(true);
        };
        return await Task.FromResult(false);
    }
}
