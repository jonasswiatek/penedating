using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model.Contract
{
    public interface IUserService
    {
        UserAccessToken Login(UserCredentials credentials);
        UserAccessToken Create(UserCredentials credentials);

        /// <summary>
        /// Meant for administrative purposes.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserAccessToken ImpersonateUser(string userId);
        IEnumerable<UserAccessToken> GetUsers();

        void DeleteUser(UserAccessToken accessToken);
    }
}