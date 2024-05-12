using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Models;

namespace Store.Application.Services;
public class ProductService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<Product>> GetAllProducts()
    {
        return await _appDbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetProductById(Guid productId)
    {
        return await Task.FromResult((await GetAllProducts()).FirstOrDefault(p => p.ProductId == productId));
    }

    public async Task<Product> CreateProduct(ProductModel newProduct)
    {
        Product product = new Product
        {
            Name = newProduct.Name,
            Price = newProduct.Price,
            Stock = newProduct.Stock,
            Description = newProduct.Description,
        };

        await _appDbContext.Products.AddAsync(product);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(product);
    }

    public async Task<Product?> UpdateProduct(Guid productId, ProductModel updateProduct)
    {
        Product? p = await GetProductById(productId);
        if (p is null) return null;

        p.Name = updateProduct.Name;
        p.Price = updateProduct.Price;
        p.Stock = updateProduct.Stock;

        await _appDbContext.SaveChangesAsync();
        return await Task.FromResult(p);
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {
        var productToDelete = await GetProductById(productId);
        if (productToDelete is null) return await Task.FromResult(false);

        _appDbContext.Products.Remove(productToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}