using DigiturkFilmAPI.Domain.Enums;

namespace DigiturkFilmAPI.Models
{
    public class FilmRequest
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string ThumbnailUrl { get; set; } = default!;
        public string VideoUrl { get; set; } = default!;
        public FilmCategory Category { get; set; } = default!;

        public void Validate ()
        {
            Title = Title.Trim();
            Description = Description?.Trim();
            ThumbnailUrl = ThumbnailUrl.Trim();
            VideoUrl = VideoUrl.Trim();

            if (Title is null || Description is null || ThumbnailUrl is null || VideoUrl is null)
                throw new Exception("Parameters can't be empty!");

            if (Category.Equals(FilmCategory.Undefined))
                throw new Exception("Parameters can't be undefined!");


        }
    }
}
