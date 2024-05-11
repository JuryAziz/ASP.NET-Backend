

namespace Store.EntityFramework.Entities;

public class ProductCategory
{

    public Guid ProductCategoryId { get; set; }


    public Guid CategoryId { get; set; }//category_id
    public Guid ProductId { get; set; }//product_id

    public required Product Product { get; set; }
    public required Category Category { get; set; }
}