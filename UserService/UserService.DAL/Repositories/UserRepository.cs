using Microsoft.EntityFrameworkCore;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.DAL.Repositories;

public class UserRepository(UserServiceDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetUserByIdentityId(string identityId)
    {
        var user = await Context.Users
            .Where(u => u.IdentityId == identityId)
            .Include(u => u.Cart)
            .FirstOrDefaultAsync();

        return user;
    }
}
