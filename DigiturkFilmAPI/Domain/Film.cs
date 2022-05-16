using DigiturkFilmAPI.Constants;
using DigiturkFilmAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DigiturkFilmAPI.Domain
{
    public class Film : BaseClass
    {
        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string ThumbnailUrl { get; set; } = default!;
        public string VideoUrl { get; set; } = default!;
        public FilmCategory Category { get; set; } = default!;  
        
    }
}
