using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model.Contract
{
    public interface IUserService
    {
        User Login(string name, string password);
        User Create(string name, string password);
        void Update(string userId, User updatedUserProfile);
    }
}