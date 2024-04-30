
using Store.Dto.Product;
using Store.Models;

namespace Store.Application.Services;


public class ProductService
{

    public readonly static List<ProductModel> _products = [
         new ProductModel
            {
                ProductId = Guid.Parse("0bb2fb13-dfe1-497d-a6f8-2d71f3640013"),
                Title = "Product 1",
                Price = 10.99m,
                TotalQuantity = 100,
                Description = "Description for Product 1",
                Thumbnail = "product1.png"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("57f5e731-fa1d-4b64-b1d6-bff4d49e127f"),
                Title = "Product 2",
                Price = 15.99m,
                TotalQuantity = 50,
                Description = "Description for Product 2",
                Thumbnail = "product2.png"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("ac83f982-5474-4782-8b8d-1a5b2c27a5f8"),
                Title = "Product 3",
                Price = 20.49m,
                TotalQuantity = 75,
                Description = "Description for Product 3",
                Thumbnail = "product3.png"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("d10eebf7-647b-478a-b527-cc738929b573"),
                Title = "Product 4",
                Price = 8.75m,
                TotalQuantity = 120,
                Description = "Description for Product 4",
                Thumbnail = "product4.png"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("41c3fbc2-dcf0-43e1-aafe-14420bf5c2a7"),
                Title = "Product 5",
                Price = 12.25m,
                TotalQuantity = 90,
                Description = "Description for Product 5",
                Thumbnail = "product5.png"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("efab54da-7e7a-4827-8ba1-f16a80e3650a"),
                Title = "Product 6",
                Price = 18.99m,
                TotalQuantity = 60,
                Description = "Description for Product 6",
                Thumbnail = "product6.png"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("a5058e82-c335-4cc2-a5e6-22e43e95b0d9"),
                Title = "Product 7",
                Price = 22.50m,
                TotalQuantity = 80,
                Description = "Description for Product 7",
                Thumbnail = "product7.png"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("c0975f56-3045-4858-bce3-29d8e78a6eb5"),
                Title = "Product 8",
                Price = 9.99m,
                TotalQuantity = 110,
                Description = "Description for Product 8",
                Thumbnail = "product8.png"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("f3637bbd-d6a0-4f3e-bf94-d665285d272e"),
                Title = "Product 9",
                Price = 14.75m,
                TotalQuantity = 70,
                Description = "Description for Product 9",
                Thumbnail = "product9.png"
            },
            new ProductModel
            {
                ProductId = Guid.Parse("46dc9f60-2b0d-41c8-bbea-5b3157f81c1b"),
                Title = "Product 10",
                Price = 16.99m,
                TotalQuantity = 85,
                Description = "Description for Product 10",
                Thumbnail = "product10.png"
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
            Title = newProduct.Title,
            Price = newProduct.Price,
            TotalQuantity = newProduct.TotalQuantity,
            Description = newProduct.Description,
            Thumbnail = newProduct.Thumbnail

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
            existingProduct.Title = updateProduct.Title;
            existingProduct.Price = updateProduct.Price;
            existingProduct.TotalQuantity = updateProduct.TotalQuantity;
            existingProduct.Description = updateProduct.Description;
            existingProduct.Thumbnail = updateProduct.Thumbnail;
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