using System.Collections.Immutable;
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


    //

    public async Task<IEnumerable<ProductModel>> Seed()
    {
        foreach (var item in _products)
        {
            await CreateProduct(item!);
        }
        return [];
    }
    public readonly static List<ProductModel> _products = [
            new ProductModel
            {
                //_productId = Guid.Parse("0bb2fb13-dfe1-497d-a6f8-2d71f3640013"),
                Name = "Product 1",
                Price = 10.99m,
                Stock = 100,
                Description = "Description for Product 1",
                CategoryList = [
                    CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a191","a11111 a1"),
                ]
            },
            new ProductModel
            {
                //_productId = Guid.Parse("57f5e731-fa1d-4b64-b1d6-bff4d49e127f"),
                Name = "Product 2",
                Price = 15.99m,
                Stock = 50,
                Description = "Description for Product 2",
                CategoryList = [
                   CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a181","a11111 a1"),
                ]
            },
            new ProductModel
            {
                //_productId = Guid.Parse("ac83f982-5474-4782-8b8d-1a5b2c27a5f8"),
                Name = "Product 3",
                Price = 20.49m,
                Stock = 75,
                Description = "Description for Product 3",
                CategoryList = [
                    CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a117","a11111 a1"),
                ]
            },
            new ProductModel
            {
                //_productId = Guid.Parse("d10eebf7-647b-478a-b527-cc738929b573"),
                Name = "Product 4",
                Price = 8.75m,
                Stock = 120,
                Description = "Description for Product 4",
                CategoryList = [
                    CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a611","a11111 a1")
                ]
            },
            new ProductModel
            {
                //_productId = Guid.Parse("41c3fbc2-dcf0-43e1-aafe-14420bf5c2a7"),
                Name = "Product 5",
                Price = 12.25m,
                Stock = 90,
                Description = "Description for Product 5",
                CategoryList = [
                    CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a151","a11111 a1"),
                ]
            },
            new ProductModel
            {
                //_productId = Guid.Parse("efab54da-7e7a-4827-8ba1-f16a80e3650a"),
                Name = "Product 6",
                Price = 18.99m,
                Stock = 60,
                Description = "Description for Product 6",
                CategoryList = [
                    CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a114","a11111 a1")
                ]
            },
            new ProductModel
            {
                //_productId = Guid.Parse("a5058e82-c335-4cc2-a5e6-22e43e95b0d9"),
                Name = "Product 7",
                Price = 22.50m,
                Stock = 80,
                Description = "Description for Product 7",
                CategoryList = [
                    CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a131","a11111 a1")
                ]
            },
            new ProductModel
            {
                //_productId = Guid.Parse("c0975f56-3045-4858-bce3-29d8e78a6eb5"),
                Name = "Product 8",
                Price = 9.99m,
                Stock = 110,
                Description = "Description for Product 8",
                CategoryList = [
                    CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a211","a11111 a1")
                ]
            },
            new ProductModel
            {
                //_productId = Guid.Parse("f3637bbd-d6a0-4f3e-bf94-d665285d272e"),
                Name = "Product 9",
                Price = 14.75m,
                Stock = 70,
                Description = "Description for Product 9",
                CategoryList = [
                    CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a112","a11111 a1")
                ]
            },
            new ProductModel
            {
                //_productId = Guid.Parse("46dc9f60-2b0d-41c8-bbea-5b3157f81c1b"),
                Name = "Product 10",
                Price = 16.99m,
                Stock = 85,
                Description = "Description for Product 10",
                CategoryList = [
                    CategoryModel.Create("a1","a1 a1 "),
                    CategoryModel.Create("a111","a11111 a1")
                ]
            }
    ];
}