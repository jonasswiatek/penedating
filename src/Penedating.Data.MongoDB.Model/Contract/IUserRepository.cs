using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Data.MongoDB.Model.Contract
{
    public interface IUserRepository
    {
        User Login(string email, byte[] passwordHash);
        User Create(string email, byte[] passwordHash);
        User GetUserByID(string userId);
    }
}