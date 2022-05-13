using DigiturkFilmAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DigiturkFilmAPI.Middleware
{

    public class JwtMiddleware
    {
        //private readonly RequestDelegate _next;

        //public JwtMiddleware(RequestDelegate next)
        //{
        //    _next = next;
        //}

        //public async Task Invoke(HttpContext context, UserService userService, IJwtUtils jwtUtils)
        //{
        //    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = jwtUtils.ValidateToken(token);
        //    if (userId != null)
        //    {
        //        // attach user to context on successful jwt validation
        //        context.Items["User"] = userService.GetUserById(userId.Value);
        //    }

        //    await _next(context);
        //}

        //private int? ValidateToken(string token)
        //{
        //    if (token == null)
        //        return null;

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //    try
        //    {
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        //            ClockSkew = TimeSpan.Zero
        //        }, out SecurityToken validatedToken);

        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

        //        // return user id from JWT token if validation successful
        //        return userId;
        //    }
        //    catch
        //    {
        //        // return null if validation fails
        //        return null;
        //    }
        //}
    }
}
