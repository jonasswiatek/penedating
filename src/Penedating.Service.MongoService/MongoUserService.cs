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

namespace Penedating.Service.MongoService
{
    public class MongoUserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public MongoUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserAccessToken Login(UserCredentials credentials)
        {
            try
            {
                var user = _userRepository.Login(credentials.Email, credentials.Password);

                return Mapper.Map<UserAccessToken>(user);
            }
            catch(UserEntityNotFoundException entityNotFoundException)
            {
                throw new InvalidUserCredentialsException();
            }

        }

        public UserAccessToken Create(UserCredentials credentials)
        {
            try
            {
                var user = _userRepository.Create(credentials.Email, credentials.Password);

                return Mapper.Map<UserAccessToken>(user);
            }
            catch(UserEntityAlreadyExistsException userEntityAlreadyExists)
            {
                throw new UserExistsException();
            }
        }

        public UserAccessToken ImpersonateUser(string userId)
        {
            try
            {
                var user = _userRepository.GetUserByID(userId);

                return Mapper.Map<UserAccessToken>(user);
            }
            catch (UserEntityNotFoundException entityNotFoundException)
            {
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
        }
    }
}