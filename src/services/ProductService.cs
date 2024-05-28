using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Dtos;
using Store.EntityFramework;
using Store.EntityFramework.Entities;

namespace Store.Application.Services;
public class ProductService(AppDbContext appDbContext, IMapper mapper)
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        return (await _appDbContext.Products
        .Include(p => p.Category)
        .ToListAsync()).Select(_mapper.Map<ProductDto>);
    }

    public async Task<Product?> GetProductById(Guid productId)
    {
        return await Task.FromResult(_appDbContext.Products
        .FirstOrDefault(p => p.ProductId == productId));
    }

    public async Task<ProductDto> CreateProduct(CreateProductDto newProduct)
    {
        Product product = new()
        {
            Name = newProduct.Name,
            Price = newProduct.Price,
            Stock = newProduct.Stock,
            Description = newProduct.Description,
        };

        Category? category = _appDbContext.Categories.FirstOrDefault(category => category.Name == newProduct.Category);
        category?.Products.Add(product);

        await _appDbContext.Products.AddAsync(product);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(_mapper.Map<ProductDto>(product));
    }

    public async Task<ProductDto?> UpdateProduct(Guid productId, UpdateProductDto updatedProduct)
    {
        Product? product = await GetProductById(productId);
        if (product is null) return null;

        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.Price = updatedProduct.Price;
        product.Stock = updatedProduct.Stock;

        Category? category = _appDbContext.Categories.FirstOrDefault(category => category.Name == updatedProduct.Category);
        product.Category = category;

        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(_mapper.Map<ProductDto>(product));
    }

    public async Task<DeleteProductDto?> DeleteProduct(Guid productId)
    {
        Product? productToDelete = await GetProductById(productId);
        if (productToDelete is null) return null;

        _appDbContext.Products.Remove(productToDelete);
        await _appDbContext.SaveChangesAsync();

        DeleteProductDto? deletedProduct = _mapper.Map<DeleteProductDto>(productToDelete);

        return await Task.FromResult(deletedProduct);
    }
}