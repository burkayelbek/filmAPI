using DigiturkFilmAPI.Domain;
using DigiturkFilmAPI.Models;
using DigiturkFilmAPI.Stores;

namespace DigiturkFilmAPI.Services
{
    public class UserService
    {
        private DataStore _store;

        public UserService(DataStore store)
        {
            _store = store;
        }

        public User GetUserById(string id)
        {
            User? foundUser = _store.users.FirstOrDefault(u => u.Id.ToString() == id);

            if (foundUser is null)
                throw new Exception($"Could not find the user with the id: {id}");

            return foundUser;
        } 
    }
}
