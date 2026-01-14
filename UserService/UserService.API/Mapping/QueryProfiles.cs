using AutoMapper;
using UserService.BLL.Carts.Queries.GetUserCart;
using UserService.BLL.Users.Queries.GetUserById;

namespace UserService.API.Mapping;

public class QueryProfiles : Profile
{
    public QueryProfiles()
    {
        CreateMap<string, GetUserByIdQuery>()
            .ForMember(dest => dest.Auth0Id, opt => opt.MapFrom(src => src));
        CreateMap<string, GetUserCartQuery>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src));
    }
}
