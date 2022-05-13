using DigiturkFilmAPI.Domain.Enums;

namespace DigiturkFilmAPI.Domain
{
    public class Film
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString();
        public string Title { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string ThumbnailUrl { get; set; } = default!;
        public string VideoUrl { get; set; } = default!;
        public FilmCategory Category { get; set; } = default!;
    }
}
