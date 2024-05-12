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
    }
}