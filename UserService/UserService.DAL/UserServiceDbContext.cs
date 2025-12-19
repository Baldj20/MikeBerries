using Microsoft.EntityFrameworkCore;
using UserService.DAL.Entities;

namespace UserService.DAL;

public class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
}
