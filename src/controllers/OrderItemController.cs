
using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.entityFramework;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;

[ApiController]
[Route("/api/OrderItem")]
public class OrderItemController : ControllerBase
{
    private readonly OrderItemService _OrderItemService;

    public OrderItemController(AppDbContext appDbContext)
    {
        _OrderItemService = new OrderItemService(appDbContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderItems([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {
            if (limit > 20) limit = 20;
            IEnumerable<OrderItem>? OrderItems = await _OrderItemService.GetOrderItems(page, limit);
            var response = new BaseResponseList<OrderItem>(OrderItems, true);
            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetOrderItems' page {page} limit {limit}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{orderItemId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetOrderItemsById(string orderItemId)
    {
        try
        {
            if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid OrderItem ID Format"));

            OrderItem? foundOrderItems = await _OrderItemService.GetOrderItemById(orderItemIdGuid);
            if (foundOrderItems is null) return NotFound();
            return Ok(new BaseResponse<OrderItem>(foundOrderItems, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetOrderItemsById'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderItem(OrderItemModel newOrderItem)
    {
        try
        {
            OrderItem? createdOrderItem = await _OrderItemService.CreateOrderItems(newOrderItem);
            return CreatedAtAction(nameof(GetOrderItemsById), new { createdOrderItem?.OrderItemId }, createdOrderItem);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'CreateOrderItem'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{orderItemId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdateOrderItem(string orderItemId, OrderItemModel newOrderItem)
    {
        try
        {
            if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid OrderItem ID Format"));

            OrderItem? OrderItemToBeUpdated = await _OrderItemService.GetOrderItemById(orderItemIdGuid);
            if (OrderItemToBeUpdated is null) return NotFound();
            await _OrderItemService.UpdateOrderItems(orderItemIdGuid, newOrderItem);
            return Ok(new BaseResponse<OrderItem>(OrderItemToBeUpdated, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'UpdateOrderItem'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{orderItemId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeleteOrderItem(string orderItemId)
    {
        try
        {
            if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid OrderItem ID Format"));
            OrderItem? orderItemToDelete = await _OrderItemService.GetOrderItemById(orderItemIdGuid);
            if (orderItemToDelete is null || !await _OrderItemService.DeleteOrderItem(orderItemIdGuid)) return NotFound();
            return Ok(new BaseResponse<OrderItem>(orderItemToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'DeleteOrderItem'");
            return StatusCode(500, ex.Message);
        }
    }

}
