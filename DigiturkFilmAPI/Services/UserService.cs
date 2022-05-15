using DigiturkFilmAPI.Domain;
using DigiturkFilmAPI.Models;
using DigiturkFilmAPI.Stores;

namespace DigiturkFilmAPI.Services
{
    public class UserService
    {
        private DataStore _dataStore;

        public UserService(DataStore dataStore)
        {
            _dataStore = dataStore;
        }


        public List<User> GetAll()
        {
            return _dataStore.GetAll<User>();
        }

        public User GetById(string id)
        {
            return _dataStore.GetById<User>(id);
        }

        public bool Delete(string id)
        {
            _dataStore.Delete<User>(id);
            return true;

        }
    }
}
