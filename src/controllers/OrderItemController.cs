using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Models;
using AutoMapper;
using System.Security.Claims;
using Store.Helpers.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/orderitems")]
public class OrderItemController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
{
    private readonly OrderItemService _orderItemService = new (appDbContext);
    private readonly AuthSerivce _authService = new (appDbContext, mapper);

    #pragma warning disable CS8604 // Possible null reference argument.

    [HttpGet]
    public async Task<IActionResult> GetOrderItems([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        List<OrderItem> orderItems = await _orderItemService.GetOrderItems();
        PaginationResult<OrderItem> paginatedOrderItems = new() { Items = orderItems.Skip((pageNumber - 1) * pageSize).Take(pageSize), TotalCount = orderItems.Count, PageNumber = pageNumber, PageSize = pageSize };
        return Ok(new BaseResponse<PaginationResult<OrderItem>>(paginatedOrderItems, true));
    }

    [HttpGet("{orderItemId}")]
    public async Task<IActionResult> GetOrderItemsById(string orderItemId)
    {
        if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid OrderItem ID Format"));

        OrderItem? foundOrderItems = await _orderItemService.GetOrderItemById(orderItemIdGuid);
        if (foundOrderItems is null) return NotFound();

        return Ok(new BaseResponse<OrderItem>(foundOrderItems, true));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrderItem(OrderItemModel newOrderItem)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        OrderItem? createdOrderItem = await _orderItemService.CreateOrderItems(newOrderItem);
        return CreatedAtAction(nameof(GetOrderItemsById), new { createdOrderItem?.OrderItemId }, createdOrderItem);
    }

    [Authorize]
    [HttpPut("{orderItemId}")]
    public async Task<IActionResult> UpdateOrderItem(string orderItemId, OrderItemModel newOrderItem)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid OrderItem ID Format"));

        OrderItem? orderItemToBeUpdated = await _orderItemService.GetOrderItemById(orderItemIdGuid);
        if (orderItemToBeUpdated is null) return NotFound();
        OrderItem? updatedOrderItem = await _orderItemService.UpdateOrderItems(orderItemIdGuid, newOrderItem);
        
        return Ok(new BaseResponse<OrderItem>(updatedOrderItem, true));
    }

    [Authorize]
    [HttpDelete("{orderItemId}")]
    public async Task<IActionResult> DeleteOrderItem(string orderItemId)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(orderItemId, out Guid orderItemIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid OrderItem ID Format"));
        
        OrderItem? orderItemToDelete = await _orderItemService.GetOrderItemById(orderItemIdGuid);
        if (orderItemToDelete is null || !await _orderItemService.DeleteOrderItem(orderItemIdGuid)) return NotFound();
        
        return Ok(new BaseResponse<OrderItem>(orderItemToDelete, true));
    }
}
