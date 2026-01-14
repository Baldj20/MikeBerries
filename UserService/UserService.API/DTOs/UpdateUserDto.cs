namespace UserService.API.DTOs;

public class UpdateUserDto
{
    public required string Auth0Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}
