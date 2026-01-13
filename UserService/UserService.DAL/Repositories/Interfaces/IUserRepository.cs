using UserService.DAL.Entities;

namespace UserService.DAL.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByAuth0Id(string auth0Id);
}
