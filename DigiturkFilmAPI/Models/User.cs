namespace DigiturkFilmAPI.Models
{
    public class User
    {
        public Guid Id { get; } = System.Guid.NewGuid();
        public string Username { get; set; } = default!;
        public byte[] PasswordHash { get; set; } = default!;
        public byte[] PasswordSalt { get; set; } = default!;
    }
}
