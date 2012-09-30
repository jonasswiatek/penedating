using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Data.MongoDB.Model.Contract
{
    public interface IUserRepository
    {
        User Login(string email, string password);
        User Create(string email, string password);
        User GetUserByID(string userId);
    }
}