using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;

[ApiController]
[Route("/api/shoppinglists")]
public class ShoppingListsController(ShoppingListService shoppingListService) : ControllerBase
{
    private readonly ShoppingListService _shoppingListService = shoppingListService;

    [HttpGet]
    public async Task<IActionResult> GetShoppingLists([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {
            if (limit > 20) limit = 20;
            IEnumerable<ShoppingListModel>? shoppingLists = await _shoppingListService.GetShoppingLists(page, limit);
            return Ok(new BaseResponseList<ShoppingListModel>(shoppingLists, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'GetShoppingLists' page {page} limit {limit}");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{shoppingListId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetShoppingListById(string shoppingListId)
    {
        try
        {
            if (!Guid.TryParse(shoppingListId, out Guid shoppingListIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid ShoppingList ID Format"));
            ShoppingListModel? foundShoppingList = await _shoppingListService.GetShoppingListById(shoppingListIdGuid);
            
            if (foundShoppingList is null) return NotFound();
            return Ok(new BaseResponse<ShoppingListModel>(foundShoppingList, true));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'GetShoppingListById'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateShoppingList(ShoppingListModel newShoppingList)
    {
        try
        {
            ShoppingListModel? createdShoppingList = await _shoppingListService.CreateShoppingList(newShoppingList);
            return CreatedAtAction(nameof(GetShoppingListById), new { createdShoppingList?.ShoppingListId }, createdShoppingList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'GetShoppingListById'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{shoppingListId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdateShoppingList(string shoppingListId, ShoppingListModel rawUpdatedShoppingList)
    {
        try
        {
            if (!Guid.TryParse(shoppingListId, out Guid shoppingListIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid ShoppingList ID Format"));
            ShoppingListModel? ShoppingListToUpdate = await _shoppingListService.GetShoppingListById(shoppingListIdGuid);

            if (ShoppingListToUpdate is null) return NotFound();
            await _shoppingListService.UpdateShoppingList(shoppingListIdGuid, rawUpdatedShoppingList);

            return Ok(new BaseResponse<ShoppingListModel>(ShoppingListToUpdate, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'UpdateShoppingList'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{shoppingListId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeleteShoppingList(string shoppingListId)
    {
        try
        {
            if (!Guid.TryParse(shoppingListId, out Guid shoppingListIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid ShoppingList ID Format"));
            ShoppingListModel? ShoppingListToDelete = await _shoppingListService.GetShoppingListById(shoppingListIdGuid);

            if (ShoppingListToDelete is null || !await _shoppingListService.DeleteShoppingList(shoppingListIdGuid)) return NotFound();
            return Ok(new BaseResponse<ShoppingListModel>(ShoppingListToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'DeleteShoppingList'");
            return StatusCode(500, ex.Message);
        }
    }

}