using DigiturkFilmAPI.Constants;
using DigiturkFilmAPI.Domain;
using DigiturkFilmAPI.Models;
using System.Text.Json;

namespace DigiturkFilmAPI.Stores
{
    public class DataStore
    {
        public List<User> users = new List<User>();
        public List<Film> films = new List<Film>();

        private readonly IConfiguration _configuration;
        private readonly string _fileLocation = "";

        public DataStore(IConfiguration configuration)
        {
            _configuration = configuration;
            _fileLocation = _configuration.GetSection("DataStorage:Location").Value;
            SeedData(DigiturkFilmConstants.UserFileName);
            SeedData(DigiturkFilmConstants.FilmFileName);
        }

        private void SeedData(string fileName)
        {
            if (!fileName.Contains(DigiturkFilmConstants.UserFileName) && !fileName.Contains(DigiturkFilmConstants.FilmFileName))
                throw new Exception("Invalid file name or un-handled!");

            CreateDirectoryIfNotExist();
            CreateFileIfNotExist(fileName);
            string storedData = File.ReadAllText(_fileLocation + fileName);


            switch (fileName)
            {
                case DigiturkFilmConstants.UserFileName:
                    users = JsonSerializer.Deserialize<List<User>>(storedData);
                    break;
                case DigiturkFilmConstants.FilmFileName:
                    films = JsonSerializer.Deserialize<List<Film>>(storedData);
                    break;
            }
        }

        private void CreateDirectoryIfNotExist()
        {
            // We need to create the file if it does not already exist.
            if (!Directory.Exists(_fileLocation))
                Directory.CreateDirectory(_fileLocation);
        }

        private void CreateFileIfNotExist(string fileName)
        {
            if (!File.Exists(fileName))
            {
                FileStream stream = File.Create(_fileLocation + fileName);

                switch(fileName)
                {
                    case DigiturkFilmConstants.UserFileName:
                        users.Add(SetDefaultUserData());
                        JsonSerializer.SerializeAsync(stream, users).Wait();
                        break;
                    case DigiturkFilmConstants.FilmFileName:
                        films.AddRange(SetDefaultFilmData());
                        JsonSerializer.SerializeAsync(stream, films).Wait();
                        break;
                    default: throw new Exception("Given file name is missing logic for creation"); break;
                }

                stream.Dispose();
            }
        }
             
        private User SetDefaultUserData()
        {
            // Password is : admin123
            return new User
            {
                Username = "admin",
                PasswordHash = "2b3c2fc2be92aa2351593b4c8ca19239673924c3c6c08c9137aee37c221d7382",
                PasswordSalt = "dfa282864394992766619782dd1b25b57e8d4a66ca50fd2505e9f76e54b987005948af6c57df6f313d9cd84e276f63ba80488c8cbdb050b1ffedeb98ff8cbd05"
            };
        }

        private List<Film> SetDefaultFilmData()
        {

            return new List<Film>
            {   
                new Film
                {
                    Title = "Comedy film",
                    Description = "This is a description about a film",
                    ThumbnailUrl = "[insert URL]",
                    VideoUrl = "[insert URL]",
                    Category = Domain.Enums.FilmCategory.Comedy,
                },
                new Film
                {
                    Title = "Drama film",
                    Description = "This is a description about a film",
                    ThumbnailUrl = "[insert URL]",
                    VideoUrl = "[insert URL]",
                    Category = Domain.Enums.FilmCategory.Drama,
                },
                new Film
                {
                    Title = "Action film",
                    Description = "This is a description about a film",
                    ThumbnailUrl = "[insert URL]",
                    VideoUrl = "[insert URL]",
                    Category = Domain.Enums.FilmCategory.Action,
                }
            };
        }

    }
}
