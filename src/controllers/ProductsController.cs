using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Dtos;
using AutoMapper;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/products")]
public class ProductsController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
{
    private readonly ProductService _productService = new(appDbContext, mapper);

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50, [FromQuery] string sortBy = "Name", [FromQuery] string dir = "Asc")
    {
        IEnumerable<ProductDto> products = await _productService.GetAllProducts();
        IEnumerable<ProductDto> sortedProducts = products;

        switch (dir.ToLower())
        {
            case "asc":
                switch (sortBy.ToLower())
                {
                    case "name":
                        sortedProducts = [.. sortedProducts.OrderBy(p => p.Name)];
                        break;
                    case "price":
                        sortedProducts = [.. sortedProducts.OrderBy(p => p.Price)];
                        break;
                    case "category":
                        sortedProducts = [.. sortedProducts.OrderBy(p => p.Categories?.ToArray()[0].Name)];
                        break;
                    case "stock":
                        sortedProducts = [.. sortedProducts.OrderBy(p => p.Stock)];
                        break;
                }
                break;
            case "desc":
                switch (sortBy.ToLower())
                {
                    case "name":
                        sortedProducts = [.. sortedProducts.OrderByDescending(p => p.Name)];
                        break;
                    case "price":
                        sortedProducts = [.. sortedProducts.OrderByDescending(p => p.Price)];
                        break;
                    case "category":
                        sortedProducts = [.. sortedProducts.OrderByDescending(p => p.Categories?.ToArray()[0].Name)];
                        break;
                    case "stock":
                        sortedProducts = [.. sortedProducts.OrderByDescending(p => p.Stock)];
                        break;
                }
                break;
        }

        PaginationResult<ProductDto> paginatedProducts = new() { Items = sortedProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize), TotalCount = products.Count(), PageNumber = pageNumber, PageSize = pageSize };

        return Ok(new BaseResponse<PaginationResult<ProductDto>>(paginatedProducts, true));
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
    public async Task<IActionResult> CreateProduct(CreateProductDto newProduct)
    {
        ProductDto? createdProduct = await _productService.CreateProduct(newProduct);
        return CreatedAtAction(nameof(GetProductById), new { createdProduct?.ProductId }, createdProduct);
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(string productId, UpdateProductDto updateProduct)
    {

        if (!Guid.TryParse(productId, out Guid productIdGuid)) return BadRequest(new BaseResponse<object>(null, false, "Invalid product ID Format"));

        Product? productToUpdate = await _productService.GetProductById(productIdGuid);
        if (productToUpdate is null) return BadRequest(ModelState);

        ProductDto? updatedProduct = await _productService.UpdateProduct(productIdGuid, updateProduct);

        return Ok(new BaseResponse<ProductDto>(updatedProduct, true));


    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(string productId)
    {

        if (!Guid.TryParse(productId, out Guid productIdGuid)) return BadRequest("Invalid product ID Format");

        Product? productToDelete = await _productService.GetProductById(productIdGuid);
        if (productToDelete is null) return NotFound();
        DeleteProductDto? deletedProduct = await _productService.DeleteProduct(productIdGuid);
        if (deletedProduct is null) return NotFound();

        return Ok(new BaseResponse<DeleteProductDto>(deletedProduct, true));
    }
}