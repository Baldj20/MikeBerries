using AutoMapper;
using UserService.API.DTOs;
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
    }
}
