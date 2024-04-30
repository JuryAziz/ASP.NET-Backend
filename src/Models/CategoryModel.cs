using System.Text.Json.Serialization;

namespace Store.Models;

public class CategoryModel
{
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Guid? _categoryId = null;


    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    private IEnumerable<ProductModel>? _productEntityList { get; set; }



    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid CategoryId
    {
        get => _categoryId ?? default;
    }




    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<ProductModel>? ProductEntityList
    {
        get
        {
            return _productEntityList;
        }
    }



    /*
        public static CategoryModel FromEntity(Category category)
    */


}