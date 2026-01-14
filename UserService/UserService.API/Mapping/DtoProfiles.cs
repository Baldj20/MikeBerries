using AutoMapper;
using UserService.API.DTOs;
using UserService.DAL.Entities;

namespace UserService.API.Mapping;

public class DtoProfiles : Profile
{
    public DtoProfiles()
    {
        CreateMap<User, GetUserDto>();
    }
}
