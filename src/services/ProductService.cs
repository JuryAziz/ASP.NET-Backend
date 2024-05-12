using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Models;

namespace Store.Application.Services;
public class ProductService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<PaginationResult<ProductModel>> GetAllProducts(string? search, int page = 1, int limit = 20)
    {
        var skip = (page - 1) * limit;
        IQueryable<Product> q = _appDbContext.Products.Skip(skip).Take(limit).Include(e => e.CategoryList);
        if (search != null)
        {
            q = q.Where(e => e.Name.Contains(search));
        }

        var totalProductsCount = await _appDbContext.Products.CountAsync();
        IEnumerable<Product> list = await q.ToListAsync();
        IEnumerable<ProductModel> productModelList = list.Select(e => ProductModel.FromEntity(e));

        return new PaginationResult<ProductModel>
        {
            Items = productModelList,
            TotalCount = totalProductsCount,
            PageNumber = page,
            PageSize = limit
        };
    }

    public async Task<ProductModel?> GetProductById(Guid productId)
    {
        Product? product = await _appDbContext.Products.Where(e => e.ProductId == productId).Include(e => e.CategoryList).SingleOrDefaultAsync();
        return product != null ? ProductModel.FromEntity(product) : null;
    }

    public async Task<ProductModel> CreateProduct(ProductModel newProduct)
    {
        ProductModel productTemplet = new ProductModel
        {
            _productId = null,
            Name = newProduct.Name,
            Price = newProduct.Price,
            Stock = newProduct.Stock,
            Description = newProduct.Description,
            CategoryList = newProduct.CategoryList
        };
        Product productEntity = Product.Create(productTemplet);
        await _appDbContext.Products.AddAsync(productEntity);

        // this section for add categoies
        if (productTemplet.CategoryList != null)
        {
            foreach (var item in productTemplet.CategoryList)
            {
                Category? category = await _appDbContext.Categories.Where(e => e.Name == item.Name).SingleOrDefaultAsync();
                if (category == null)
                {
                    category = Category.FromModel(item);
                    await _appDbContext.Categories.AddAsync(category);
                }

                ProductCategory? productCategory = await _appDbContext.ProductCategories
                .Where(e => e.ProductId == productEntity.ProductId && e.CategoryId == category.CategoryId)
                .SingleOrDefaultAsync();

                if (productCategory != null)
                {
                    continue;
                }
                await _appDbContext.ProductCategories.AddAsync(new ProductCategory
                {
                    Product = productEntity,
                    Category = category
                });
            }
        }

        await _appDbContext.SaveChangesAsync();
        return productTemplet;
    }

    public async Task<ProductModel?> UpdateProduct(Guid productId, ProductModel updateProduct)
    {
        Product? p = await _appDbContext.Products.Where(e => e.ProductId == productId).FirstOrDefaultAsync();

        if (p == null)
        {
            return null;
        }

        p.Name = updateProduct.Name;
        p.Price = updateProduct.Price;
        p.Stock = updateProduct.Stock;

        _appDbContext.Products.Update(p);
        await _appDbContext.SaveChangesAsync();
        return ProductModel.FromEntity(p);
    }

    public async Task<bool> DeleteProduct(Guid productId)
    {

        int _deleted = await _appDbContext.Products.Where(e => e.ProductId == productId).ExecuteDeleteAsync();
        return _deleted == 1;
    }
}