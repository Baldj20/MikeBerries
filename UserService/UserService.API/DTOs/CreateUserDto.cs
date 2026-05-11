namespace UserService.API.DTOs;

public class CreateUserDto
{
    public required string IdentityId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}
