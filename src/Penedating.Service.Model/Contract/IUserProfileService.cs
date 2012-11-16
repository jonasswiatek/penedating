using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model.Contract
{
    public interface IUserProfileService
    {
        MultipageResponse<UserProfile> GetProfiles(int pageIndex, int pageSize);
        
        UserProfile GetUserProfile(UserAccessToken userCredentials);
        void UpdateProfile(UserAccessToken accessToken, UserProfile updatedProfile);
    }
}
