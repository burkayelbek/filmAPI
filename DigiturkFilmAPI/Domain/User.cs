using DigiturkFilmAPI.Constants;

namespace DigiturkFilmAPI.Domain
{
    public class User : BaseClass
    {
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string PasswordSalt { get; set; } = default!;

    }
}
