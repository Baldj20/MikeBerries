using AutoMapper;
using UserService.BLL.Users.Queries.GetUserById;

namespace UserService.API.Mapping;

public class QueryProfiles : Profile
{
    public QueryProfiles()
    {
        CreateMap<string, GetUserByIdQuery>()
            .ForMember(dest => dest.Auth0Id, opt => opt.MapFrom(src => src));
    }
}
