namespace Api.Models
{
    public class LoginRequestDto
    {
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
    }

    public class LoginResponseDto
    {
        public string AccessToken { get; init; } = default!;
        public int ExpiresIn { get; init; }
    }
}
