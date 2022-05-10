namespace DigiturkFilmAPI.Models
{
    public class LoginRequest
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;

        public void ValidateObject()
        {
            Username = Username?.Trim();
            Password = Password?.Trim();

            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
                throw new ArgumentException("Username or password can't be empty");
        }
    }
}
