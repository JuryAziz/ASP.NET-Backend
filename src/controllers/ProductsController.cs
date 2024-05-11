using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Store.Application.Services;

using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/products")]
public class ProductsController(ProductService productService) : ControllerBase
{
    private readonly ProductService _productService = productService;

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] string? q, [FromQuery] int page = 1)
    {
        try
        {
            if (page <= 0)
            {
                return BadRequest(
                    new BaseResponse<object>(success: false, msg: "page most be more then 0 ")
                );
            }
            PaginationResult<ProductModel> products = await _productService.GetAllProducts(q, page);

            return Ok(new BaseResponse<PaginationResult<ProductModel>>(products, true));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
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