using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Data.MongoDB.Model.Contract
{
    public interface IUserRepository
    {
        User Login(string username, byte[] passwordHash);
        User Create(string username, byte[] passwordHash);
        User Update(User user);
        User GetUserByID(string userId);
    }
}