using DigiturkFilmAPI.Constants;
using DigiturkFilmAPI.Domain;
using DigiturkFilmAPI.Models;
using DigiturkFilmAPI.Stores;
using System.Text.Json;

namespace DigiturkFilmAPI.Services
{
    public class FilmService
    {
        private readonly DataStore _dataStore;

        public FilmService(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public Film Create (FilmRequest request)
        {
            request.Validate();
            return PostFilm (request);
        }

        public List<Film?> GetAll()
        {
            return _dataStore.GetAll<Film>();
        }

        public Film GetById(string id)
        {
            return _dataStore.GetById<Film>(id);
        }

        public bool Delete(string id)
        {
            return _dataStore.Delete<Film>(id);
        }

        public Film Patch(Film request)
        {
            
            return _dataStore.Update<Film>(new Film
            {
                Id = request.Id,
                Category = request.Category,
                Description = request.Description,
                ThumbnailUrl = request.ThumbnailUrl,
                Title = request.Title,
                VideoUrl = request.VideoUrl
            });
        }

        private Film PostFilm(FilmRequest request)
        {
            Film film = new Film()
            {
                Category = request.Category,
                Description = request.Description,
                ThumbnailUrl = request.ThumbnailUrl,
                Title = request.Title,
                VideoUrl = request.VideoUrl,
            };

            return _dataStore.Add(film);
        }
        
    }
}
