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

        UserProfile GetUserProfile(UserCredentials userCredentials);
        void UpdateProfile(UserAccessToken accessToken, UserProfile updatedProfile);
    }
}