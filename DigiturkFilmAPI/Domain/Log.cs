namespace DigiturkFilmAPI.Domain
{
    public class Log : BaseClass
    {
        public string Text { get; set; } = default!;
        public string? RawData { get; set; } = default!;
        public LogLevel Level { get; set; }
        public string ObjectType { get; set; } = default!;
        public DateTime CreatedAt { get; } = DateTime.Now;
    }
}
