using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Store.Application.Services;

using Store.Helpers;
using Store.Models;

[ApiController]
[Route("/api/products")]
public class ProductsController(ProductService productService) : ControllerBase
{
    private readonly ProductService _productService = productService;

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        IEnumerable<ProductModel> products = await _productService.GetAllProductsService();
        return Ok(new BaseResponseList<ProductModel>(products, true));
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProduct(string productId)
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

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Default)] ProductModel newProduct)
    {
        var createdProduct = await _productService.CreateProductService(newProduct);



        return CreatedAtAction(nameof(GetProduct), new { productId = createdProduct.ProductId }, createdProduct);
    }



    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(string productId, [FromBody] ProductModel updateProduct)
    {


        if (!Guid.TryParse(productId, out Guid productIdGuid))
        {
            return BadRequest(new BaseResponse<object>(null, false, "Invalid product ID Format"));
        }

        if (updateProduct == null)
            return BadRequest();

        var product = await _productService.UpdateProductService(productIdGuid, updateProduct);
        if (product == null)
        {
            //
            return NotFound();
        }
        return Ok(product);
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProduct(string productId)
    {
        if (!Guid.TryParse(productId, out Guid productIdGuid))
        {
            return BadRequest("Invalid product ID Format");
        }
        var result = await _productService.DeleteProductService(productIdGuid);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    // added search function
    [HttpGet("search/{keywords}")]
    public async Task<IActionResult> SearchProductByNameOrDescription(string keywords)
    {
        var result = await _productService.SearchProductByNameOrDescription(keywords);
        if (result == null) return NotFound();
        return Ok(new BaseResponseList<ProductModel>(result, true));
    }
}