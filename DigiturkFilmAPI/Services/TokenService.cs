using DigiturkFilmAPI.Domain;
using DigiturkFilmAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
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

        /*
         * The hashing and encoding logic was referenced by the link 'https://stackoverflow.com/questions/12185122/calculating-hmacsha256-using-c-sharp-to-match-payment-provider-example'.
         * However the logic was improved upon by storing the salt on the user rather than storing a static string for enhancing security.
         * 
         */

        public (string hash, string salt) CreatePasswordHash(string password)
        {
            byte[] tempSalt = null;

            using (var hmac = new HMACSHA256())
            {
                var byteSalt = hmac.Key;
                tempSalt = byteSalt;
            }

            byte[] hash = HashHMAC(tempSalt, StringEncode(password));
            string computedHash = HashEncode(hash);

            return (computedHash, HashEncode(tempSalt));
        }

        public bool VerifyPasswordHash(User user, string password)
        {
            byte[] hash = HashHMAC(HexDecode(user.PasswordSalt), StringEncode(password));
            string computedHash = HashEncode(hash);
            return computedHash.SequenceEqual(user.PasswordHash);
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

        #region Hash Hex Functions
        private static string HashHMACHex(string keyHex, string message)
        {
            byte[] hash = HashHMAC(HexDecode(keyHex), StringEncode(message));
            return HashEncode(hash);
        }

        #endregion

        #region Hash Functions
        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }

        #endregion

        #region Encoding Helpers
        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }
 
        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
 
        private static byte[] HexDecode(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }
        #endregion
    }
}
