namespace UniversitySystem.Application.Features.Auth.Commands.Login;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public long UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string[] Roles { get; set; } = [];
}