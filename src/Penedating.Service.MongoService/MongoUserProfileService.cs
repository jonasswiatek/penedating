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