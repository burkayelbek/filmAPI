using DigiturkFilmAPI.Domain;
using DigiturkFilmAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DigiturkFilmAPI.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string hash, string salt) CreatePasswordHash(string password)
        {
            byte[] tempSalt = null;
            byte[] tempHash = null;


            using (var hmac = new HMACSHA256())
            {
                tempSalt = hmac.Key;
                tempHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return (hash: Convert.ToBase64String(tempHash), salt: Convert.ToBase64String(tempSalt));
        }

        public bool VerifyPasswordHash(User user, string password)
        {
            using (var hmac = new HMACSHA256(System.Text.Encoding.ASCII.GetBytes(user.PasswordSalt)))
            {
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        public string CreateToken(User user)
        {

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("UserID", user.Id.ToString()),
                        new Claim("Name", user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTSettings:Key").Value)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
