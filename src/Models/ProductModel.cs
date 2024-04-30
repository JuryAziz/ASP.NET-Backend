using System.Text.Json.Serialization;

namespace Store.Models;

public class ProductModel
{
    public required Guid ProductId { get; set; }

    public required string Title { get; set; }
    public required Decimal Price { get; set; }
    public required int TotalQuantity { get; set; }
    public string? Description { get; set; }

    public string? Thumbnail { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<CategoryModel>? CategoryEntityList { get; set; }
}




/**

product_id     uuid DEFAULT gen_random_uuid() NOT NULL,
title          varchar NOT NULL,
price          float4  NOT NULL,
total_quantity integer NOT NULL,
description    text    NOT NULL,
thumbnail      varchar NOT NULL,
PRIMARY KEY (product_id)
**/
// !!
//public required Guid CategoryId { get; set; }
//public required Guid MerchantId { get; set; }