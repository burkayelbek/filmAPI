﻿using DigiturkFilmAPI.Models;

namespace DigiturkFilmAPI.Stores
{
    public class DataStore
    {
        public List<User> users = new List<User>();

        public DataStore()
        {
            seedUser();
        }

        private void seedUser()
        {
            // Password: admin123
            users.Add(new User
            {
                Username = "admin",
                PasswordHash = new byte[] { 241, 38, 243, 51, 247, 2, 97, 240, 74, 65, 204, 137, 163, 13, 72, 110, 202, 157, 109, 19, 75, 229, 125, 117, 219, 139, 111, 2, 188, 138, 1, 225, 32, 221, 89, 125, 136, 84, 146, 220, 44, 81, 38, 241, 11, 12, 84, 128, 248, 19, 35, 153, 5, 37, 251, 29, 140, 123, 87, 59, 56, 116, 84, 26 },
                PasswordSalt = new byte[] { 125, 72, 109, 234, 9, 231, 173, 34, 174, 59, 178, 205, 234, 221, 50, 198, 237, 119, 233, 45, 44, 139, 84, 25, 38, 136, 109, 196, 106, 190, 40, 255, 244, 44, 177, 138, 182, 78, 10, 223, 251, 62, 47, 15, 151, 50, 247, 37, 102, 19, 102, 93, 185, 82, 173, 148, 243, 237, 156, 67, 244, 121, 210, 93, 246, 194, 45, 48, 252, 33, 188, 94, 172, 194, 201, 184, 42, 47, 12, 49, 70, 96, 15, 250, 247, 254, 1, 103, 67, 221, 60, 76, 47, 158, 45, 2, 114, 205, 153, 44, 193, 215, 31, 85, 225, 254, 84, 79, 239, 234, 110, 73, 106, 99, 55, 175, 91, 125, 62, 202, 22, 55, 225, 255, 191, 174, 49, 161 }
            });
        }

    }
}