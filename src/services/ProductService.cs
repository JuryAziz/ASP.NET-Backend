
using Store.Dto.Product;
using Store.Models;

namespace Store.Application.Services;


public class ProductService
{

    public readonly static List<ProductModel> _products = [
         new ProductModel
            {
                ProductId = Guid.Parse("0bb2fb13-dfe1-497d-a6f8-2d71f3640013"),
                Name = "Product 1",
                Price = 10.99m,
                Stock = 100,
                Description = "Description for Product 1"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("57f5e731-fa1d-4b64-b1d6-bff4d49e127f"),
                Name = "Product 2",
                Price = 15.99m,
                Stock = 50,
                Description = "Description for Product 2"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("ac83f982-5474-4782-8b8d-1a5b2c27a5f8"),
                Name = "Product 3",
                Price = 20.49m,
                Stock = 75,
                Description = "Description for Product 3"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("d10eebf7-647b-478a-b527-cc738929b573"),
                Name = "Product 4",
                Price = 8.75m,
                Stock = 120,
                Description = "Description for Product 4"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("41c3fbc2-dcf0-43e1-aafe-14420bf5c2a7"),
                Name = "Product 5",
                Price = 12.25m,
                Stock = 90,
                Description = "Description for Product 5"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("efab54da-7e7a-4827-8ba1-f16a80e3650a"),
                Name = "Product 6",
                Price = 18.99m,
                Stock = 60,
                Description = "Description for Product 6"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("a5058e82-c335-4cc2-a5e6-22e43e95b0d9"),
                Name = "Product 7",
                Price = 22.50m,
                Stock = 80,
                Description = "Description for Product 7"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("c0975f56-3045-4858-bce3-29d8e78a6eb5"),
                Name = "Product 8",
                Price = 9.99m,
                Stock = 110,
                Description = "Description for Product 8"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("f3637bbd-d6a0-4f3e-bf94-d665285d272e"),
                Name = "Product 9",
                Price = 14.75m,
                Stock = 70,
                Description = "Description for Product 9"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("46dc9f60-2b0d-41c8-bbea-5b3157f81c1b"),
                Name = "Product 10",
                Price = 16.99m,
                Stock = 85,
                Description = "Description for Product 10"
            }
    ];


    public async Task<IEnumerable<ProductModel>> GetAllProductsService()
    {
        await Task.Delay(1500); // simulate delay
        return _products.AsEnumerable();
    }

    public async Task<ProductModel?> GetProductById(Guid productId)
    {
        await Task.Delay(1500); // simulate delay

        return _products.Find(product => product.ProductId == productId);
    }


    public async Task<ProductModel> CreateProductService(CreateProductDto newProduct)
    {
        await Task.Delay(1500); // simulate delay

        ProductModel productTemplet = new ProductModel
        {
            ProductId = Guid.NewGuid(),
            Name = newProduct.Name,
            Price = newProduct.Price,
            Stock = newProduct.Stock,
            Description = newProduct.Description

        };



        _products.Add(productTemplet);
        return productTemplet;
    }



    public async Task<ProductModel?> UpdateProductService(Guid productId, CreateProductDto updateProduct)
    {




        await Task.Delay(1500); // simulate delay
        var existingProduct = _products.FirstOrDefault(u => u.ProductId == productId);



        if (existingProduct != null)
        {
            existingProduct.Name = updateProduct.Name;
            existingProduct.Price = updateProduct.Price;
            existingProduct.Stock = updateProduct.Stock;
            existingProduct.Description = updateProduct.Description;
        }


        return existingProduct;
    }



    public async Task<bool> DeleteProductService(Guid productId)
    {


        await Task.Delay(1500); // simulate delay
        var removeingProduct = _products.FirstOrDefault(u => u.ProductId == productId);
        if (removeingProduct != null)
        {
            _products.Remove(removeingProduct);
            return true;
        }
        return false;
    }
}