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
        private DataStore _store;
        private readonly TokenService _tokenService;

        public AuthenticationService(DataStore store, TokenService tokenService)
        {
            _store = store;
            _tokenService = tokenService;
        }

        public string Login(LoginRequest request)
        {
            // Validate that the parameters of the object is safe to use.
            request.ValidateObject();

            User? foundUser = _store.users.FirstOrDefault(u => u.Username == request.Username);

            if (foundUser is null || !_tokenService.VerifyPasswordHash(foundUser, request.Password))
                throw new Exception("Username or password is wrong... Please try again");

            return _tokenService.CreateToken(foundUser);
        }
  

        public User Register (LoginRequest request)
        {
            (byte[] hash, byte[] salt) = _tokenService.CreatePasswordHash(request.Password);

            User newUser = new User
            {
                Username = request.Username,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _store.users.Add(newUser);
            return newUser;
        }

    }
}
