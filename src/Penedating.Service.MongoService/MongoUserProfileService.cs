using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Penedating.Data.MongoDB.Model.Contract;
using Penedating.Data.MongoDB.Model.Exceptions;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Service.Model.Exceptions;
using log4net;

namespace Penedating.Service.MongoService
{
    public class MongoUserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILog _logger;

        public MongoUserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
            _logger = LogManager.GetLogger(this.GetType());
        }

        public MultipageResponse<UserProfile> GetProfiles(int pageIndex, int pageSize)
        {
            int pageCount;
            var profiles = _userProfileRepository.GetProfiles(pageIndex, pageSize, out pageCount);

            return new MultipageResponse<UserProfile>()
                       {
                           PageCount = pageCount,
                           PageIndex = pageIndex,
                           PageSize = pageSize,
                           Result = Mapper.Map<IEnumerable<Service.Model.UserProfile>>(profiles)
                       };
        }

        public UserProfile GetUserProfile(UserAccessToken userAccessToken)
        {
            var userProfile = _userProfileRepository.GetUserProfile(userAccessToken.Ticket);

            return Mapper.Map<UserProfile>(userProfile);
        }

        public void UpdateProfile(UserAccessToken accessToken, UserProfile updatedProfile)
        {
            var dataUserProfile = Mapper.Map<Data.MongoDB.Model.UserProfile>(updatedProfile);
            _userProfileRepository.UpdateProfile(accessToken.Ticket, dataUserProfile);
            _logger.Info("Updated user profile for: " + accessToken.Email);
        }
    }
}