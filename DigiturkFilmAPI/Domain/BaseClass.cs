namespace DigiturkFilmAPI.Domain
{
    public class BaseClass
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString();
    }
}
