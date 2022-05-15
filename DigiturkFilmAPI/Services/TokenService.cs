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
        private readonly string key = "57617b5d2349434b34734345635073433835777e2d244c31715535255a366773755a4d70532a5879793238235f707c4f7865753f3f446e633a21575643303f66";
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string hash, string salt) CreatePasswordHash(string password)
        {
            //string tempSalt = null;

            //using (var hmac = new HMACSHA256())
            //{
            //    var byteSalt = hmac.Key;
            //    var byteHash = System.Text.Encoding.UTF8.GetBytes(password);
            //    tempSalt = System.Text.Encoding.UTF8.GetString(byteSalt);
            //}

            byte[] hash = HashHMAC(HexDecode(key), StringEncode(password));
            string computedHash = HashEncode(hash);

            return (computedHash, computedHash);
        }

        public bool VerifyPasswordHash(User user, string password)
        {
            byte[] hash = HashHMAC(HexDecode(key), StringEncode(password));
            string computedHash = HashEncode(hash);
            return computedHash.SequenceEqual(user.PasswordHash);

            //using (var hmac = new HMACSHA256(System.Text.Encoding.UTF8.GetBytes(user.PasswordSalt)))
            //{
            //    var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            //    var computedHash = System.Text.Encoding.UTF8.GetString(hmac.ComputeHash(passwordBytes));
            //    return computedHash.SequenceEqual(user.PasswordHash);
            //}
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
