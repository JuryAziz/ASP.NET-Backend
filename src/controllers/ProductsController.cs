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
                        sortedProducts = sortedProducts.OrderBy(p => p.CategoryList?[0].Name).ToList();
                        break;
                    case "stock":
                        sortedProducts = sortedProducts.OrderBy(p => p.Stock).ToList();
                        break;
                }
                break;
            case "desc":
                switch (sortBy.ToLower())
                {
                    case "name":
                        sortedProducts = sortedProducts.OrderByDescending(p => p.Name).ToList();
                        break;
                    case "price":
                        sortedProducts = sortedProducts.OrderByDescending(p => p.Price).ToList();
                        break;
                    case "category":
                        sortedProducts = sortedProducts.OrderByDescending(p => p.CategoryList?[0].Name).ToList();
                        break;
                    case "stock":
                        sortedProducts = sortedProducts.OrderByDescending(p => p.Stock).ToList();
                        break;
                }
                break;
        }

        List<Product> paginatedProducts = Paginate.Function(sortedProducts, page, limit);
        return Ok(new BaseResponse<Product>(paginatedProducts, true));
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductById(string productId)
    {

        if (!Guid.TryParse(productId, out Guid productIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid product ID Format"));

        var product = await _productService.GetProductById(productIdGuid);
        if (product is null) return NotFound();

        return Ok(new BaseResponse<Product>(product, true));

    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductModel newProduct)
    {
        var createdProduct = await _productService.CreateProduct(newProduct);
        return CreatedAtAction(nameof(GetProductById), new { createdProduct?.ProductId }, createdProduct);
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(string productId, ProductModel updateProduct)
    {

        if (!Guid.TryParse(productId, out Guid productIdGuid)) return BadRequest(new BaseResponse<object>(null, false, "Invalid product ID Format"));

        Product? productToUpdate = await _productService.GetProductById(productIdGuid);
        if (productToUpdate is null) return BadRequest(ModelState);

        Product? updatedProduct = await _productService.UpdateProduct(productIdGuid, updateProduct);

        return Ok(new BaseResponse<Product>(updatedProduct, true));


    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(string productId)
    {

        if (!Guid.TryParse(productId, out Guid productIdGuid)) return BadRequest("Invalid product ID Format");

        Product?productToDelete = await _productService.GetProductById(productIdGuid);
        var result = await _productService.DeleteProduct(productIdGuid);
        if (!result) return NotFound();

        return Ok(new BaseResponse<Product>(productToDelete, true));
    }
}