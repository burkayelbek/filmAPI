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

        public List<Film> GetAll()
        {
            // If there are no file we will just send the empty result
            if (!File.Exists(DigiturkFilmConstants.FilmFileName))
                return new List<Film>().ToList();

            string storedData = File.ReadAllText(DigiturkFilmConstants.FilmFileName);

            List<Film> allFilms = JsonSerializer.Deserialize<List<Film>>(storedData);

            return allFilms;
        }

        public Film GetById(string id)
        {
            Film? foundFilm = GetAll().FirstOrDefault(f => f.Id == id);

            if (foundFilm is null)
                throw new Exception($"Could not find the film with the id: {id}");

            return foundFilm;
        }

        public bool Delete(string id)
        {
            List<Film> films = GetAll();
            Film filmToRemove = GetById(id);

            films.Remove(filmToRemove);
            Save(films);
            return true;

        }

        public bool Patch(FilmRequest request, string id)
        {
            request.Validate();
            List<Film> films = GetAll();
            int filmIndex = films.FindIndex(f => f.Id == id);

            if (filmIndex == -1)
                throw new Exception($"Could not find the film with the id: {id}");

            films.ElementAt(filmIndex).Title = request.Title;
            films.ElementAt(filmIndex).Description = request.Description;
            films.ElementAt(filmIndex).ThumbnailUrl = request.ThumbnailUrl;
            films.ElementAt(filmIndex).VideoUrl = request.VideoUrl;
            films.ElementAt(filmIndex).Category = request.Category;

            Save(films);
            return true;

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

            List<Film> allFilms = GetAll();
            
            allFilms.Add(film);
            Save(allFilms);

            return film;
        }

        private void Save(List<Film> films)
        {
            FileStream? stream = null;

            // We need to create the file if it does not already exist.
            if (!File.Exists(DigiturkFilmConstants.FilmFileName))
                stream = File.Create(DigiturkFilmConstants.FilmFileName);

            if (stream is null)
                stream = File.OpenWrite(DigiturkFilmConstants.FilmFileName);

            JsonSerializer.SerializeAsync(stream, films).Wait();
            stream.Dispose();
        }
    }
}
