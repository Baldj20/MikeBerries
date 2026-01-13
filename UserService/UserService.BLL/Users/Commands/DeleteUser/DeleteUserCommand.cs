namespace UserService.BLL.Users.Commands.DeleteUser;

public class DeleteUserCommand
{
    public required string Auth0Id { get; set; }
}