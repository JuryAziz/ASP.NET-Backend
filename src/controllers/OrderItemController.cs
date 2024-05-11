using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/orderitems")]
public class OrderItemController(AppDbContext appDbContext) : ControllerBase
{
    private readonly OrderItemService _orderItemService = new (appDbContext);

    [HttpGet]
    public async Task<IActionResult> GetOrderItems([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        List<OrderItem> orderItems = await _orderItemService.GetOrderItems();
        List<OrderItem> paginatedOrderItems = Paginate.Function(orderItems, page, limit);
        return Ok(new BaseResponseList<OrderItem>(paginatedOrderItems, true));
    }

    [HttpGet("{orderItemId}")]
    public async Task<IActionResult> GetOrderItemsById(string orderItemId)
    {
        if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid OrderItem ID Format"));

        OrderItem? foundOrderItems = await _orderItemService.GetOrderItemById(orderItemIdGuid);
        if (foundOrderItems is null) return NotFound();

        return Ok(new BaseResponse<OrderItem>(foundOrderItems, true));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderItem(OrderItemModel newOrderItem)
    {
        OrderItem? createdOrderItem = await _orderItemService.CreateOrderItems(newOrderItem);
        return CreatedAtAction(nameof(GetOrderItemsById), new { createdOrderItem?.OrderItemId }, createdOrderItem);
    }

    [HttpPut("{orderItemId}")]
    public async Task<IActionResult> UpdateOrderItem(string orderItemId, OrderItemModel newOrderItem)
    {
        if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid OrderItem ID Format"));

        OrderItem? orderItemToBeUpdated = await _orderItemService.GetOrderItemById(orderItemIdGuid);
        if (orderItemToBeUpdated is null) return NotFound();
        OrderItem? updatedOrderItem = await _orderItemService.UpdateOrderItems(orderItemIdGuid, newOrderItem);
        
        return Ok(new BaseResponse<OrderItem>(updatedOrderItem, true));
    }

    [HttpDelete("{orderItemId}")]
    public async Task<IActionResult> DeleteOrderItem(string orderItemId)
    {
        if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid OrderItem ID Format"));
        
        OrderItem? orderItemToDelete = await _orderItemService.GetOrderItemById(orderItemIdGuid);
        if (orderItemToDelete is null || !await _orderItemService.DeleteOrderItem(orderItemIdGuid)) return NotFound();
        
        return Ok(new BaseResponse<OrderItem>(orderItemToDelete, true));
    }
}
