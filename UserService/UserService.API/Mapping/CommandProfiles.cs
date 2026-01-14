using AutoMapper;
using UserService.API.DTOs;
using UserService.BLL.Carts.Commands.AddItemToCart;
using UserService.BLL.Carts.Commands.DeleteItemFromCart;
using UserService.BLL.Users.Commands.DeleteUser;
using UserService.BLL.Users.Commands.UpdateUser;

namespace UserService.API.Mapping;

public class CommandProfiles : Profile
{
    public CommandProfiles()
    {
        CreateMap<string, DeleteUserCommand>()
            .ForMember(dest => dest.Auth0Id, opt => opt.MapFrom(src => src));
        CreateMap<UpdateUserDto, UpdateUserCommand>();
        CreateMap<AddItemDto, AddItemToCartCommand>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CartId));
        CreateMap<DeleteItemFromCartDto, DeleteItemFromCartCommand>()
            .ForMember(dest => dest.CartItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CartId));
    }
}
