using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/carts")]
public class CartController(CartService cartService) : ControllerBase
{
    private readonly CartService _cartService = cartService;

    [HttpGet]
    public async Task<IActionResult> GetCarts([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {
            if (limit > 20) limit = 20;
            IEnumerable<CartModel> carts = await _cartService.GetCarts(page, limit);
            
            return Ok(new BaseResponseList<CartModel>(carts, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'GetCarts' page {page} limit {limit}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{cartId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetCartById(string cartId)
    {
        try
        {
            if (!Guid.TryParse(cartId, out Guid cartIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Cart ID Format"));
            CartModel? foundCart = await _cartService.GetCartById(cartIdGuid);

            if (foundCart is null) return NotFound();
            return Ok(new BaseResponse<CartModel>(foundCart, true));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'GetCartById'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCart(CartModel newCart)
    {
        try
        {
            CartModel? createdCart = await _cartService.CreateCart(newCart);
            return CreatedAtAction(nameof(GetCartById), new { createdCart?.CartId }, createdCart);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'CreateCart'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{cartId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdateCart(string cartId, CartModel rawUpdatedCart)
    {
        try
        {
            if (!Guid.TryParse(cartId, out Guid cartIdGuid)) return BadRequest("Invalid Cart ID Format");
            CartModel? cartToUpdate = await _cartService.GetCartById(cartIdGuid);

            if (cartToUpdate is null) return BadRequest(ModelState);
            CartModel? updatedCart = await _cartService.UpdateCart(cartToUpdate.CartId, rawUpdatedCart);

            return Ok(new BaseResponse<CartModel>(updatedCart, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'UpdateCart'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{cartId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeleteCart(string cartId)
    {
        try
        {
            if (!Guid.TryParse(cartId, out Guid cartIdGuid)) return BadRequest("Invalid Cart ID Format");
            CartModel? cartToDelete = await _cartService.GetCartById(cartIdGuid);

            if (cartToDelete is null || !await _cartService.DeleteCart(cartIdGuid)) return NotFound();
            return Ok(new BaseResponse<CartModel>(cartToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while'DeleteCart'");
            return StatusCode(500, ex.Message);
        }
    }

}