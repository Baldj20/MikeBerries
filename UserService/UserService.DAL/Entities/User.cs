namespace UserService.DAL.Entities;

public class User
{
    public required string IdentityId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }

    public required Cart Cart { get; set; }
}
