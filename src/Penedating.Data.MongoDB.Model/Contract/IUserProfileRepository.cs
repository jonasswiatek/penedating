using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Data.MongoDB.Model.Contract
{
    public interface IUserProfileRepository
    {
        UserProfile GetUserProfile(string userId);
        void UpdateProfile(string userId, UserProfile profile);
        IEnumerable<UserProfile> GetProfiles(int pageIndex, int pageSize, out int pageCount);
    }
}