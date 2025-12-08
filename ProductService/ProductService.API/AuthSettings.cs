namespace ProductService.API;

public class AuthSettings
{
    public const string CONFIG_SECTION_NAME = "Auth0";

    public string Authority { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
