using Store.Application.Services.Users;
using Store.Application.Services.Addresses;
using Store.Application.Services.PaymentMethods;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<PaymentMethodService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


// ==================================================================== 
// ============================ Dummy Data ============================
// ==================================================================== 

// // ===== Products =====

// List<Product> Products = [
//     new Product(Guid.NewGuid(), Users[0].Id, "Apple iPhone 15 Pro Max 256GB", "Some description here", 5999, 0, 20, 0),
//     new Product(Guid.NewGuid(), Users[0].Id, "Apple iPhone 15 Pro Max 128GB", "Some description here", 5199, 0, 15, 0),
//     new Product(Guid.NewGuid(), Users[1].Id, "Apple iPhone 15 Pro 256GB", "Some description here", 5299, 0, 20, 0),
//     new Product(Guid.NewGuid(), Users[1].Id, "Apple iPhone 15 Pro 128GB", "Some description here", 4699, 0, 20, 0),
//     new Product(Guid.NewGuid(), Users[2].Id, "Apple iPhone 15 256GB", "Some description here", 4699, 0, 20, 0),
//     new Product(Guid.NewGuid(), Users[2].Id, "Apple iPhone 15 128GB", "Some description here", 4099, 0, 20, 0)
// ];

// // ===== Carts =====

// List<CartItem> CartItems = [
//     new CartItem(Products[0].Id, 1),
//     new CartItem(Products[2].Id, 2),
//     new CartItem(Products[1].Id, 6),
//     new CartItem(Products[4].Id, 2),
//     new CartItem(Products[5].Id, 3)
// ];

// List<Cart> Carts = [
//     new Cart(Users[0].Id) {Items = [CartItems[0].Id]},
//     new Cart(Users[1].Id) {Items = [CartItems[1].Id]},
//     new Cart(Users[2].Id) {Items = [CartItems[2].Id]},
//     new Cart(Users[3].Id) {Items = [CartItems[3].Id]},
//     new Cart(Users[3].Id) {Items = [CartItems[4].Id]},
// ];

// // ===== Shopping Lists =====

// List<ShoppingList> ShoppingLists = [
//     new ShoppingList(Users[0].Id, "Wishlist", "Items to get by next month", false) { Items = [Products[0].Id] },
//     new ShoppingList(Users[1].Id, "Wishlist", "Items to get by next month", false) { Items = [Products[2].Id] },
//     new ShoppingList(Users[2].Id, "Wishlist", "Items to get by next month", false) { Items = [Products[1].Id] },
//     new ShoppingList(Users[3].Id, "Wishlist", "Items to get by next month", false) { Items = [Products[4].Id] },
//     new ShoppingList(Users[3].Id, "Wishlist", "Items to get by next month", false) { Items = [Products[5].Id] },
// ];

// // ===== Orders =====

// List<OrderItem> OrderItems = [
//     new OrderItem(Guid.NewGuid(), Products[0].Id, Products[0].Price, 1),
//     new OrderItem(Guid.NewGuid(), Products[2].Id, Products[2].Price, 1),
//     new OrderItem(Guid.NewGuid(), Products[1].Id, Products[1].Price, 1),
//     new OrderItem(Guid.NewGuid(), Products[4].Id, Products[4].Price, 1),
//     new OrderItem(Guid.NewGuid(), Products[5].Id, Products[5].Price, 1)
// ];

// List<Order> Orders = [
//     new Order(Users[0].Id, Addresses.Find(address => address.UserId == Users[0].Id).Id, PaymentMethods.Find(paymentMethod => paymentMethod.UserId == Users[0].Id).Id, 0) { OrderItems = [OrderItems[0].Id] },
//     new Order(Users[1].Id, Addresses.Find(address => address.UserId == Users[1].Id).Id, PaymentMethods.Find(paymentMethod => paymentMethod.UserId == Users[1].Id).Id, 0) { OrderItems = [OrderItems[1].Id] },
//     new Order(Users[2].Id, Addresses.Find(address => address.UserId == Users[2].Id).Id, PaymentMethods.Find(paymentMethod => paymentMethod.UserId == Users[2].Id).Id, 0) { OrderItems = [OrderItems[2].Id] },
//     new Order(Users[3].Id, Addresses.Find(address => address.UserId == Users[3].Id).Id, PaymentMethods.Find(paymentMethod => paymentMethod.UserId == Users[3].Id).Id, 0) { OrderItems = [OrderItems[3].Id] },
//     new Order(Users[3].Id, Addresses.Find(address => address.UserId == Users[3].Id).Id, PaymentMethods.Find(paymentMethod => paymentMethod.UserId == Users[3].Id).Id, 0) { OrderItems = [OrderItems[4].Id] },
// ];

// ===============================================================================
// ============================ RESTful API Endpoints ============================
// =============================================================================== 


// public class ECommereceContext : DbContext
// {
//     public DbSet<User> User { get; set; }
//     public DbSet<Address> Addresses { get; set; }
//     public DbSet<PaymentMethod> PaymentMethods { get; set; }
//     public DbSet<Product> Products { get; set; }

//     public DbSet<Cart> Carts { get; set; }
//     public DbSet<CartItem> CartItems { get; set; }

//     public DbSet<ShoppingList> ShoppingLists { get; set; }

//     public DbSet<Order> Orders { get; set; }
//     public DbSet<OrderItem> OrderItems { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         optionsBuilder.UseSqlServer(
//             @"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True");
//     }
// };



// app.MapGet("/", () => "RESTful API is working!");

// app.MapGet("/users", () => Users);
// app.MapGet("/users/{userId:guid}/", (Guid userId) => Users.Find(user => user.Id == userId));
// app.MapGet("/users/{userId:guid}/addresses", (Guid userId) => Addresses.FindAll(address => address.UserId == userId));
// app.MapGet("/users/{userId:guid}/paymentmethods", (Guid userId) => PaymentMethods.FindAll(paymentMethod => paymentMethod.UserId == userId));
// app.MapGet("/users/{userId:guid}/cart", (Guid userId) => Carts.Find(cart => cart.UserId == userId));
// app.MapGet("/users/{userId:guid}/shoppinglists", (Guid userId) => ShoppingLists.FindAll(shoppingList => shoppingList.UserId == userId));
// app.MapGet("/users/{userId:guid}/orders", (Guid userId) => Orders.FindAll(order => order.UserId == userId));
// app.MapGet("/users/{userId:guid}/orders/{orderId:guid}", (Guid userId, Guid orderId) => Orders.FindAll(order => order.UserId == userId).Find(order => order.Id == orderId));
// app.MapGet("/users/{userId:guid}/orders/{orderId:guid}/items", (Guid userId, Guid orderId) => Orders.Find(order => order.Id == orderId && order.UserId == userId)?.OrderItems.Select(item =>
// {
//     var orderItem = OrderItems.Find(oi => oi.Id == item);
//     if (orderItem == null) return null;
//     var product = Products.Find(product => product.Id == orderItem.ProductId);
//     if (product == null) return null;
//     dynamic expando = new ExpandoObject();
//     expando.id = orderItem.Id;
//     expando.orderId = orderItem.OrderId;
//     expando.productId = orderItem.ProductId;
//     expando.merchantId = product.MerchantId;
//     expando.name = product.Name;
//     expando.description = product.Description;
//     expando.price = orderItem.Price;
//     expando.quantity = orderItem.Quantity;
//     expando.createdAt = orderItem.CreatedAt;

//     return expando;
// }));

// app.MapGet("/addresses", () => Addresses);
// app.MapGet("/addresses/{addressId:guid}/", (Guid addressId) => Addresses.Find(address => address.Id == addressId));

// app.MapGet("/paymentmethods", () => PaymentMethods);
// app.MapGet("/paymentmethods/{paymentMethodId:guid}/", (Guid paymentMethodId) => PaymentMethods.Find(paymentMethod => paymentMethod.Id == paymentMethodId));

// app.MapGet("/products", () => Products);
// app.MapGet("/products/{productId:guid}/", (Guid productId) => Products.Find(product => product.Id == productId));

// app.MapGet("/carts", () => Carts);
// app.MapGet("/carts/{cartId:guid}/", (Guid cartId) => Carts.Find(cart => cart.Id == cartId));

// app.MapGet("/shoppinglists", () => ShoppingLists);
// app.MapGet("/shoppinglists/{shoppingListId:guid}/", (Guid shoppingListId) => ShoppingLists.Find(shippingList => shippingList.Id == shoppingListId));

// app.MapGet("/orders", () => Orders);
// app.MapGet("/orders/{orderId:guid}/", (Guid orderId) => Orders.Find(order => order.Id == orderId));
// app.MapGet("/order/{orderId:guid}/items", (Guid orderId) => Orders.Find(order => order.Id == orderId)?.OrderItems.Select(item =>
// {
//     var orderItem = OrderItems.Find(oi => oi.Id == item);
//     if (orderItem == null) return null;
//     var product = Products.Find(product => product.Id == orderItem.ProductId);
//     if (product == null) return null;
//     dynamic expando = new ExpandoObject();
//     expando.id = orderItem.Id;
//     expando.orderId = orderItem.OrderId;
//     expando.productId = orderItem.ProductId;
//     expando.merchantId = product.MerchantId;
//     expando.name = product.Name;
//     expando.description = product.Description;
//     expando.price = orderItem.Price;
//     expando.quantity = orderItem.Quantity;
//     expando.createdAt = orderItem.CreatedAt;

//     return expando;
// }));
