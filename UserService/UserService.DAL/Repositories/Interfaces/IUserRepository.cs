using UserService.DAL.Entities;

namespace UserService.DAL.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByIdentityId(string identityId);
}
