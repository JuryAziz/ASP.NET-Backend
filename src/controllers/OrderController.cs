
using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;

[ApiController]
[Route("/api/Orders")]
public class OrderController(OrderService orderService) : ControllerBase
{
    private readonly OrderService _Orders = orderService;

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {
            if (limit > 20) limit = 20;
            IEnumerable<OrderModel>? Orders = await _Orders.GetOrders(page, limit);
            return Ok(new BaseResponseList<OrderModel>(Orders, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetOrders' page {page} limit {limit}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{orderId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetOrdersById(string orderId)
    {
        try
        {
            if (!Guid.TryParse(orderId, out Guid orderIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Order ID Format"));
            OrderModel? foundOrders = await _Orders.GetOrderById(orderIdGuid);
            if (foundOrders is null) return NotFound();
            return Ok(new BaseResponse<OrderModel>(foundOrders, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetOrdersById'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderModel newOrder)
    {
        try
        {
            OrderModel? createdOrder = await _Orders.CreateOrders(newOrder);
            return CreatedAtAction(nameof(GetOrdersById), new { createdOrder?.OrderId }, createdOrder);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'CreateOrder'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{orderId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdateOrder(string orderId, OrderModel newOrder)
    {
        try
        {
            if (!Guid.TryParse(orderId, out Guid orderIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Order ID Format"));
            OrderModel? OrderToBeUpdated = await _Orders.GetOrderById(orderIdGuid);
            if (OrderToBeUpdated is null) return NotFound();
            await _Orders.UpdateOrders(orderIdGuid, newOrder);
            return Ok(new BaseResponse<OrderModel>(OrderToBeUpdated, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'UpdateOrder'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{orderId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeleteOrder(string orderId)
    {
        try
        {
            if (!Guid.TryParse(orderId, out Guid orderIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Order ID Format"));
            OrderModel? orderToDelete = await _Orders.GetOrderById(orderIdGuid);
            if (orderToDelete is null || !await _Orders.DeleteOrder(orderIdGuid)) return NotFound();
            return Ok(new BaseResponse<OrderModel>(orderToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'DeleteOrder'");
            return StatusCode(500, ex.Message);
        }
    }

}
