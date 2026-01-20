using UserService.BLL.Common;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, User>
{
    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetUserByAuth0Id(request.Auth0Id);
    }
}
