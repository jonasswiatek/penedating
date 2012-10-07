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
    public class MongoUserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILog _logger;

        public MongoUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _logger = LogManager.GetLogger(this.GetType());
        }

        public UserAccessToken Login(UserCredentials credentials)
        {
            try
            {
                var user = _userRepository.Login(credentials.Email, credentials.Password);
                _logger.Info("Successful Authentication for user: " + credentials.Email);
                if(user.IsAdmin)
                {
                    _logger.Warn("User " + credentials.Email + " has been granted administrative rights");
                }
                return Mapper.Map<UserAccessToken>(user);
            }
            catch(UserEntityNotFoundException entityNotFoundException)
            {
                _logger.Warn("Failed Authentication for user: " + credentials.Email);
                throw new InvalidUserCredentialsException();
            }

        }

        public UserAccessToken Create(UserCredentials credentials)
        {
            try
            {
                var user = _userRepository.Create(credentials.Email, credentials.Password);
                _logger.Info("Created user account: " + credentials.Email);

                return Mapper.Map<UserAccessToken>(user);
            }
            catch(UserEntityAlreadyExistsException userEntityAlreadyExists)
            {
                _logger.Info("Failed to create account, it already existed: " + credentials.Email);
                throw new UserExistsException();
            }
        }

        public UserAccessToken ImpersonateUser(string userId)
        {
            try
            {
                var user = _userRepository.GetUserByID(userId);

                _logger.Warn("Impersonating user: " + userId);
                return Mapper.Map<UserAccessToken>(user);
            }
            catch (UserEntityNotFoundException entityNotFoundException)
            {
                _logger.Warn("Attempted to impersonate nonexistent user: " + userId);
                throw new InvalidUserCredentialsException();
            }
        }

        public IEnumerable<UserAccessToken> GetUsers()
        {
            return _userRepository.GetAllUsers().Select(Mapper.Map<UserAccessToken>);
        }

        public UserProfile GetUserProfile(UserAccessToken userAccessToken)
        {
            var userProfile = _userRepository.GetUserProfile(userAccessToken.Ticket);

            return Mapper.Map<UserProfile>(userProfile);
        }

        public void UpdateProfile(UserAccessToken accessToken, UserProfile updatedProfile)
        {
            var dataUserProfile = Mapper.Map<Data.MongoDB.Model.UserProfile>(updatedProfile);
            _userRepository.UpdateProfile(accessToken.Ticket, dataUserProfile);
            _logger.Info("Updated user profile for: " + accessToken.Email);
        }

        public void DeleteUser(UserAccessToken accessToken)
        {
            _userRepository.DeleteUser(accessToken.Ticket);
            _logger.Info("Deleted user account: " + accessToken.Email);
        }
    }
}