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
        CreateMap<UpdateUserDto, UpdateUserCommand>();
        CreateMap<AddItemDto, AddItemToCartCommand>();
    }
}
