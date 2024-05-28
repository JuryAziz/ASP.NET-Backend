using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Store.Application.Services;
using Store.EntityFramework;
using Store.EntityFramework.Entities;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

var DefaultConnection = Environment.GetEnvironmentVariable("DefaultConnection") ?? throw new Exception("DefaultConnection is not set in .env file");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(DefaultConnection));

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderItemService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<PaymentMethodService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoriesService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// connecting api to frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // react link here
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var JwtKey = Environment.GetEnvironmentVariable("Jwt__Key") ?? throw new Exception("Jwt__Key is not set in .env file");
var JwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? throw new Exception("Jwt__Issuer is not set in .env file");
var JwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience") ?? throw new Exception("Jwt__Audience is not set in .env file");

var key = Encoding.ASCII.GetBytes(JwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = JwtIssuer,
        ValidAudience = JwtAudience,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();
app.Run();