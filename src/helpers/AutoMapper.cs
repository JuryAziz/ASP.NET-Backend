using AutoMapper;
using Store.Dtos;
using Store.EntityFramework.Entities;

namespace Store.Helpers;

public class AutoMapper : Profile 
{
    public AutoMapper() 
    {
        // Input => Output
        CreateMap<RegisterDto, UserDto>();
        
        CreateMap<User, UserDto>();
        CreateMap<User, DeleteUserDto>();

        CreateMap<CreateAddressDto, AddressDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<Address, DeleteAddressDto>();

        CreateMap<CreatePaymentMethodDto, PaymentMethodDto>();
        CreateMap<PaymentMethod, PaymentMethodDto>();
        CreateMap<PaymentMethod, DeletePaymentMethodDto>();

        CreateMap<CreateCategoryDto, CategoryDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Category, DeleteCategoryDto>();

        CreateMap<CreateProductDto, ProductDto>();
        CreateMap<Product, ProductDto>();
        CreateMap<Product, DeleteProductDto>();

        CreateMap<CreateOrderDto, OrderDto>();
        CreateMap<Order, OrderDto>();
        CreateMap<Order, DeleteOrderDto>();
    }
}