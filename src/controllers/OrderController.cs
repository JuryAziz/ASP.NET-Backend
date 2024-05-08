
using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.entityFramework;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;

[ApiController]
[Route("/api/Orders")]
public class OrderController : ControllerBase
{
    private readonly OrderService _OrderService;

    public OrderController(AppDbContext appDbContext)
    {
        _OrderService = new OrderService(appDbContext);
    }


    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {
            if (limit > 20) limit = 20;
            IEnumerable<Order>? Orders = await _OrderService.GetOrders(page, limit);
            var response = new BaseResponseList<Order>(Orders, true);
            return Ok(response);
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

            Order? foundOrders = await _OrderService.GetOrderById(orderIdGuid);
            if (foundOrders is null) return NotFound();
            return Ok(new BaseResponse<Order>(foundOrders, true));
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
            Order? createdOrder = await _OrderService.CreateOrders(newOrder);
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

            Order? OrderToBeUpdated = await _OrderService.GetOrderById(orderIdGuid);
            if (OrderToBeUpdated is null) return NotFound();
            await _OrderService.UpdateOrders(orderIdGuid, newOrder);
            return Ok(new BaseResponse<Order>(OrderToBeUpdated, true));
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
            Order? orderToDelete = await _OrderService.GetOrderById(orderIdGuid);
            if (orderToDelete is null || !await _OrderService.DeleteOrder(orderIdGuid)) return NotFound();
            return Ok(new BaseResponse<Order>(orderToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'DeleteOrder'");
            return StatusCode(500, ex.Message);
        }
    }

}
