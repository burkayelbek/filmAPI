namespace DigiturkFilmAPI.Domain
{
    public class User
    {
        public Guid Id { get; } = System.Guid.NewGuid();
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string PasswordSalt { get; set; } = default!;
    }
}
