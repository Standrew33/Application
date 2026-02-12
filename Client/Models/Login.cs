namespace Client.Models
{
    public class LoginRequestData
    {
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
    }

    public class LoginResponseData
    {
        public string AccessToken { get; init; } = default!;
        public int ExpiresIn { get; init; }
    }
}
