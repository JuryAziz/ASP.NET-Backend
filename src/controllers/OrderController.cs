using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Helpers.Enums;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/orders")]
public class OrderController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
{
    private readonly OrderService _orderService = new (appDbContext);
    private readonly AuthSerivce _authService = new (appDbContext, mapper);

    #pragma warning disable CS8604 // Possible null reference argument.

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        List<Order> orders = await _orderService.GetOrders();
        List<Order> paginatedOrders = Paginate.Function(orders, page, limit);
        return Ok(new BaseResponseList<Order>(paginatedOrders, true));
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrdersById(string orderId)
    {
        if (!Guid.TryParse(orderId, out Guid orderIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Order ID Format"));

        Order? foundOrders = await _orderService.GetOrderById(orderIdGuid);
        if (foundOrders is null) return NotFound();

        return Ok(new BaseResponse<Order>(foundOrders, true));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderModel newOrder)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        Order? createdOrder = await _orderService.CreateOrders(newOrder);
        return CreatedAtAction(nameof(GetOrdersById), new { createdOrder?.OrderId }, createdOrder);
    }

    [Authorize]
    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrder(string orderId, OrderModel newOrder)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(orderId, out Guid orderIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Order ID Format"));

        Order? orderToBeUpdated = await _orderService.GetOrderById(orderIdGuid);
        if (orderToBeUpdated is null) return NotFound();
        Order? updatedOrder = await _orderService.UpdateOrders(orderIdGuid, newOrder);

        return Ok(new BaseResponse<Order>(updatedOrder, true));
    }

    [Authorize]
    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(string orderId)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(orderId, out Guid orderIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Order ID Format"));

        Order? orderToDelete = await _orderService.GetOrderById(orderIdGuid);
        if (orderToDelete is null || !await _orderService.DeleteOrder(orderIdGuid)) return NotFound();
        
        return Ok(new BaseResponse<Order>(orderToDelete, true));
    }
}
