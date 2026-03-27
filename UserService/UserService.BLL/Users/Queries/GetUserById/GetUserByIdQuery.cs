using MediatR;
using UserService.DAL.Entities;

namespace UserService.BLL.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<User>
{
    public required string IdentityId { get; set; }
}

