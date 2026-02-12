using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("login")]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto request)
        {
            if (!(request.Username == "admin" && request.Password == "12345"))
                return Unauthorized(new { message = "Incorrect login or password" });

            var jwt = _configuration.GetSection("Jwt");
            var expired = int.Parse(jwt["ExpiresMinutes"]!);

            //Statement about user
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, request.Username),
                new (ClaimTypes.Name, request.Username)
            };

            //Symmetric - one key for both signing and verification
            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"]!,
                audience: jwt["Audience"]!,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expired),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)), 
                    SecurityAlgorithms.HmacSha256)
            );

            return Ok(new LoginResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),  //serialization + Base64 encoding
                ExpiresIn = (int)TimeSpan.FromMinutes(expired).TotalSeconds
            });
        }
    }
}
