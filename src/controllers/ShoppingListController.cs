using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/shoppinglists")]
public class ShoppingListController(AppDbContext appDbContext) : ControllerBase
{
    private readonly ShoppingListService _shoppingListService = new(appDbContext);

    [HttpGet]
    public async Task<IActionResult> GetShoppingLists([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        List<ShoppingList> shoppingLists = await _shoppingListService.GetShoppingLists();
        List<ShoppingList> paginatedShoppingLists = Paginate.Function(shoppingLists, page, limit);
        return Ok(new BaseResponseList<ShoppingList>(paginatedShoppingLists, true));
    }

    [HttpGet("{shoppingListId}")]
    public async Task<IActionResult> GetShoppingListById(string shoppingListId)
    {
        if (!Guid.TryParse(shoppingListId, out Guid shoppingListIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid ShoppingList ID Format"));

        ShoppingList? foundShoppingLists = await _shoppingListService.GetShoppingListById(shoppingListIdGuid);
        if (foundShoppingLists is null) return NotFound();

        return Ok(new BaseResponse<ShoppingList>(foundShoppingLists, true));
    }

    [HttpPost]
    public async Task<IActionResult> CreateShoppingList(ShoppingListModel newShoppingList)
    {
        ShoppingList? createdShoppingList = await _shoppingListService.CreateShoppingList(newShoppingList);
        return CreatedAtAction(nameof(GetShoppingListById), new { createdShoppingList?.ShoppingListId }, createdShoppingList);
    }

    [HttpPut("{shoppingListId}")]
    public async Task<IActionResult> UpdateShoppingList(string shoppingListId, ShoppingListModel newShoppingList)
    {
        if (!Guid.TryParse(shoppingListId, out Guid shoppingListIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid ShoppingList ID Format"));

        ShoppingList? shoppingListToBeUpdated = await _shoppingListService.GetShoppingListById(shoppingListIdGuid);
        if (shoppingListToBeUpdated is null) return NotFound();
        ShoppingList? updatedShoppingList = await _shoppingListService.UpdateShoppingList(shoppingListIdGuid, newShoppingList);

        return Ok(new BaseResponse<ShoppingList>(updatedShoppingList, true));
    }

    [HttpDelete("{shoppingListId}")]
    public async Task<IActionResult> DeleteShoppingList(string shoppingListId)
    {
        if (!Guid.TryParse(shoppingListId, out Guid shoppingListIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid ShoppingList ID Format"));

        ShoppingList? shoppingListToDelete = await _shoppingListService.GetShoppingListById(shoppingListIdGuid);
        if (shoppingListToDelete is null || !await _shoppingListService.DeleteShoppingList(shoppingListIdGuid)) return NotFound();

        return Ok(new BaseResponse<ShoppingList>(shoppingListToDelete, true));
    }
}
