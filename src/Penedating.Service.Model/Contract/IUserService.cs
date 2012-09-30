using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model.Contract
{
    public interface IUserService
    {
        UserAccessToken Login(UserCredentials credentials);
        UserAccessToken Create(UserCredentials credentials);

        void UpdateProfile(UserAccessToken accessToken, UserProfile updatedProfile);
    }
}