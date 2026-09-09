using Model.Entities;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace API.Authentication
{
    /// <summary>
    /// tạo token khi đăng nhập thành công
    /// </summary>
    public class JwtTokenProvider
    {
        private readonly IConfiguration _configuration;
        public JwtTokenProvider(IConfiguration config)
        {
            _configuration = config;
        }
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            string secretKey =
                _configuration["Jwt:SecretKey"]
                ?? throw new Exception("Chưa cấu hình Jwt:SecretKey");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials( key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
}
