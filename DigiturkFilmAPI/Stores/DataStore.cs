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
        public List<Log> logs = new List<Log>();

        public DataStore()
        {
            SeedData(DigiturkFilmConstants.UserFileName);
            SeedData(DigiturkFilmConstants.FilmFileName);
            SeedData(DigiturkFilmConstants.LogFileName);
        }

        public T Add<T>(T obj) where T : BaseClass 
        {
            try
            {
                List<T?> data = GetAll<T>();

                data.Add(obj);
                Save(data);
            }
            catch (Exception ex) { LogData("DataStore.Add: Could not add the data", JsonSerializer.Serialize(obj), LogLevel.Error, typeof(T).FullName); }

            return obj;
        }

        public bool Delete<T>(string id) where T : BaseClass
        {

            try
            {
                List<T?> data = GetAll<T>();
                T dataRemove = GetById<T>(id);

                bool succeeded = data.Remove(dataRemove);
                Save(data);

                if (!succeeded)
                    LogData("DataStore.Delete: Deletion did not succeed", id, LogLevel.Warning, typeof(T).FullName);
                else
                    LogData("DataStore.Delete: Item deleted", JsonSerializer.Serialize(dataRemove), LogLevel.Information, typeof(T).FullName);

                return succeeded;
            } catch { LogData("DataStore.Delete: Could not delete the data", id, LogLevel.Error, typeof(T).FullName); }

            return false;
        }

        public List<T?> GetAll<T>() where T : BaseClass
        {
            List<T?> allData = null;
            try
            {
                // If there are no file we will just send the empty result
                if (!File.Exists(GetFullFilePath<T>()))
                    return new List<T?>().ToList();

                string storedData = File.ReadAllText(GetFullFilePath<T>());

                allData = JsonSerializer.Deserialize<List<T?>>(storedData);
            } catch { LogData("DataStore.GetAll: Could not get the data", String.Empty, LogLevel.Error, typeof(T).FullName); }
            

            return allData;
        }

        public T GetById<T>(string id) where T : BaseClass
        {
            T? foundData = null;
            try
            {
                foundData = GetByIdOrDefault<T>(id);

                if (foundData is null)
                {
                    LogData("DataStore.GetById: Could not find the data", id, LogLevel.Warning, typeof(T).FullName);
                    throw new Exception($"Could not find the data with the id: {id}");
                }

            } catch { LogData("DataStore.GetById: Could not get the data", id, LogLevel.Error, typeof(T).FullName); }
            

            return foundData;
        }

        public T? GetByIdOrDefault<T>(string id) where T : BaseClass
        {
            T? foundData = GetAll<T>().FirstOrDefault(f => f.Id == id);
            return foundData;
        }

        public T Update<T>(T obj) where T : BaseClass
        {
            List<T?>? allData = null;
            int foundDataIndex = -1;
            try
            {
                allData = GetAll<T>();
                foundDataIndex = allData.FindIndex(T => T.Id == obj.Id);

                if (foundDataIndex == -1)
                {
                    LogData("DataStore.Update: Could not find the data", JsonSerializer.Serialize(obj), LogLevel.Warning, typeof(T).FullName);
                    throw new Exception($"Failed to update, could not find the data with the id: {obj.Id} ");
                }

                allData[foundDataIndex] = obj;

                Save(allData);
            } catch { LogData("DataStore.Update: Could not update the data", JsonSerializer.Serialize(obj), LogLevel.Error, typeof(T).FullName); }


            return allData[foundDataIndex];
        }


        public void Save<T>(List<T?> objs) where T : BaseClass
        {
            try
            {
                FileStream? stream = null;

                // We need to create the file if it does not already exist.
                if (!File.Exists(GetFullFilePath<T>()))
                    stream = File.Create(GetFullFilePath<T>());

                if (stream is null)
                    stream = File.OpenWrite(GetFullFilePath<T>());

                JsonSerializer.SerializeAsync(stream, objs).Wait();
                stream.Dispose();
            } catch { LogData("DataStore.Save: Could not save the data", JsonSerializer.Serialize(objs), LogLevel.Error, typeof(T).FullName); }
            
        }

        private void LogData(string text, string rawData, LogLevel level, string objectType)
        {
            List<Log?> allData = null;
            // If there are no file we will just send the empty result
            if (!File.Exists(GetFullFilePath<Log>()))
                return;

            string storedData = File.ReadAllText(GetFullFilePath<Log>());

            allData = JsonSerializer.Deserialize<List<Log?>>(storedData);

            allData.Add(new Log
            {
                Level = level,
                Text = text,
                RawData = rawData,
                ObjectType = objectType
            });

            FileStream? stream = null;

            // We need to create the file if it does not already exist.
            if (!File.Exists(GetFullFilePath<Log>()))
                stream = File.Create(GetFullFilePath<Log>());

            if (stream is null)
                stream = File.OpenWrite(GetFullFilePath<Log>());

            JsonSerializer.SerializeAsync(stream, allData).Wait();
            stream.Dispose();
        }

        private void SeedData(string fileName)
        {
            if (!fileName.Contains(DigiturkFilmConstants.UserFileName) && !fileName.Contains(DigiturkFilmConstants.FilmFileName) && !fileName.Contains(DigiturkFilmConstants.LogFileName))
                throw new Exception("Invalid file name or un-handled!");

            CreateDirectoryIfNotExist();
            CreateFileIfNotExist(fileName);
            string storedData = File.ReadAllText(DigiturkFilmConstants.DirectoryPath + fileName);

            if (storedData != "[]")
                return;


            switch (fileName)
            {
                case DigiturkFilmConstants.UserFileName:
                    users = JsonSerializer.Deserialize<List<User>>(storedData);
                    break;
                case DigiturkFilmConstants.FilmFileName:
                    films = JsonSerializer.Deserialize<List<Film>>(storedData);
                    break;
                case DigiturkFilmConstants.LogFileName:
                    logs = JsonSerializer.Deserialize<List<Log>>(storedData);
                    break;
            }
        }

        private void CreateDirectoryIfNotExist()
        {
            // We need to create the file if it does not already exist.
            if (!Directory.Exists(DigiturkFilmConstants.DirectoryPath))
                Directory.CreateDirectory(DigiturkFilmConstants.DirectoryPath);
        }

        private void CreateFileIfNotExist(string fileName)
        {
            if (!File.Exists(DigiturkFilmConstants.DirectoryPath + fileName))
            {
                FileStream stream = File.Create(DigiturkFilmConstants.DirectoryPath + fileName);

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
                    case DigiturkFilmConstants.LogFileName:
                        logs.AddRange(SetDefaultLogData());
                        JsonSerializer.SerializeAsync(stream, logs).Wait();
                        break;
                    default: throw new Exception("Given file name is missing logic for creation");
                }

                stream.Dispose();
            }
        }

        private string GetFullFilePath<T>()
        {
            if (typeof(T).FullName == typeof(User).FullName)
                return DigiturkFilmConstants.DirectoryPath + DigiturkFilmConstants.UserFileName;
            else if (typeof(T).FullName == typeof(Film).FullName)
                return DigiturkFilmConstants.DirectoryPath + DigiturkFilmConstants.FilmFileName;
            else if (typeof(T).FullName == typeof(Log).FullName)
                return DigiturkFilmConstants.DirectoryPath + DigiturkFilmConstants.LogFileName;
            else
                throw new Exception("Given generic is not a valid");
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

        private List<Log> SetDefaultLogData()
        {
            return new List<Log>();
        }

    }
}
