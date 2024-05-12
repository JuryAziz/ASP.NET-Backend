using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/products")]
public class ProductsController(AppDbContext appDbContext) : ControllerBase
{
    private readonly ProductService _productService = new(appDbContext);

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] int page = 1, [FromQuery] int limit = 50, [FromQuery] string sortBy = "Name", [FromQuery] string dir = "Asc")
    {
        List<Product> products = await _productService.GetAllProducts();

        List<Product> sortedProducts = products;
        switch (dir.ToLower())
        {
            case "asc":
                switch (sortBy.ToLower())
                {
                    case "name":
                        sortedProducts = sortedProducts.OrderBy(p => p.Name).ToList();
                        break;
                    case "price":
                        sortedProducts = sortedProducts.OrderBy(p => p.Price).ToList();
                        break;
                    case "category":
                        sortedProducts = sortedProducts.OrderBy(p => p.CategoryList.Get).ToList();
                        break;
                    case "createdat":
                        sortedProducts = sortedProducts.OrderBy(p => p.CreatedAt).ToList();
                        break;
                }
                break;
            case "desc":
                switch (sortBy.ToLower())
                {
                    case "name":
                        sortedProducts = sortedProducts.OrderByDescending(u => u.FirstName).ToList();
                        break;
                    case "orders":
                        sortedProducts = sortedProducts.OrderByDescending(u => u.Orders?.Count).ToList();
                        break;
                    case "email":
                        sortedProducts = sortedProducts.OrderByDescending(u => u.Email).ToList();
                        break;
                    case "createdat":
                        sortedProducts = sortedProducts.OrderByDescending(u => u.CreatedAt).ToList();
                        break;
                }
                break;
        }
        List<Product> paginatedProducts = Paginate.Function(sortedProducts, page, limit);
        return Ok(new BaseResponse<Product>(paginatedProducts, true));


    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(string productId)
    {
        try
        {
            if (!Guid.TryParse(productId, out Guid productIdGuid))
            {
                return BadRequest(new BaseResponse<object>(false, "Invalid product ID Format"));
            }

            var product = await _productService.GetProductById(productIdGuid);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new BaseResponse<ProductModel>(product, true));
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Default)] ProductModel newProduct)
    {
        try
        {
            var createdProduct = await _productService.CreateProduct(newProduct);
            return CreatedAtAction(nameof(GetProduct), new { productId = createdProduct.ProductId }, createdProduct);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(string productId, [FromBody] ProductModel updateProduct)
    {
        try
        {
            if (!Guid.TryParse(productId, out Guid productIdGuid))
            {
                return BadRequest(new BaseResponse<object>(null, false, "Invalid product ID Format"));
            }

            if (updateProduct == null)
                return BadRequest();

            var product = await _productService.UpdateProduct(productIdGuid, updateProduct);
            if (product == null)
            {
                //
                return NotFound();
            }
            return Ok(product);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(string productId)
    {
        try
        {
            if (!Guid.TryParse(productId, out Guid productIdGuid))
            {
                return BadRequest("Invalid product ID Format");
            }
            var result = await _productService.DeleteProduct(productIdGuid);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        try
        {
            await _productService.Seed();
            return Created();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}