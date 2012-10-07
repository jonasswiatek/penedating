using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Data.MongoDB.Model.Contract
{
    public interface IUserRepository
    {
        User Login(string email, string password);
        User Create(string email, string password);
        void DeleteUser(string userId);

        User GetUserByID(string userId);
        IEnumerable<User> GetAllUsers();

        UserProfile GetUserProfile(string userId);
        void UpdateProfile(string userId, UserProfile profile);
    }
}