using Microsoft.EntityFrameworkCore;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.DAL.Repositories;

public class UserRepository(UserServiceDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetUserByAuth0Id(string auth0Id)
    {
        var user = await Context.Users
            .Where(u => u.Auth0Id == auth0Id)
            .Include(u => u.Cart)
            .FirstOrDefaultAsync();

        return user;
    }
}
