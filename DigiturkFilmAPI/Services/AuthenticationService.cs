using DigiturkFilmAPI.Models;
using DigiturkFilmAPI.Stores;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DigiturkFilmAPI.Services
{
    public class AuthenticationService
    {
        private UserStore _userStore;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserStore userStore, IConfiguration configuration)
        {
            _userStore = userStore;
            _configuration = configuration;
        }

        public string Login(LoginRequest request)
        {
            // Validate that the parameters of the object is safe to use.
            request.ValidateObject();

            User? foundUser = _userStore.users.FirstOrDefault(u => u.Username == request.Username);

            if (foundUser is null || !VerifyPasswordHash(foundUser, request.Password))
                throw new Exception("Username or password is wrong... Please try again");

            return CreateToken(foundUser);
        }
  

        public User Register (LoginRequest request)
        {
            (byte[] hash, byte[] salt) = CreatePasswordHash(request.Password);

            User newUser = new User
            {
                Username = request.Username,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _userStore.users.Add(newUser);
            return newUser;
        }

        private (byte[] hash, byte[] salt) CreatePasswordHash(string password)
        {
            byte[] tempSalt = null;
            byte[] tempHash = null;


            using (var hmac = new HMACSHA512())
            {
                tempSalt = hmac.Key;
                tempHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return (hash: tempHash, salt: tempSalt);
        }

        private bool VerifyPasswordHash(User user, string password)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JWTSettings:Key").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
