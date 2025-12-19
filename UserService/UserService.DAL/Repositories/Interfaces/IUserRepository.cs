using UserService.DAL.Entities;

namespace UserService.DAL.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByAuth0Id(string auth0Id);
}
