using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Penedating.Data.MongoDB.InternalModel;
using Penedating.Data.MongoDB.Model;
using Penedating.Data.MongoDB.Model.Contract;
using Penedating.Data.MongoDB.Model.Exceptions;

namespace Penedating.Data.MongoDB
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoCollection<PenedatingMongoUser> _mongoCollection;
        private readonly MongoDatabase _database;
        private readonly MongoServer _server;

        public UserRepository(string connectionString, string databaseName)
        {
            _server = MongoServer.Create(connectionString);
            _database = _server.GetDatabase(databaseName);
            _mongoCollection = _database.GetCollection<PenedatingMongoUser>("users");
        }

        public User Login(string username, string password)
        {
            var query = _mongoCollection.AsQueryable();
            var mongoUser = query.SingleOrDefault(a => a.Email == username);
            if(mongoUser == null)
            {
                throw new UserEntityNotFoundException();
            }

            if (!PasswordHash.ValidatePassword(password, mongoUser.PasswordHash))
            {
                throw new UserEntityNotFoundException();
            }

            return new User()
                       {
                           UserID = mongoUser.UserID.ToString(),
                           Email = mongoUser.Email
                       };
        }

        public User Create(string email, string password)
        {
            var query = _mongoCollection.AsQueryable();
            var existingUser = query.Any(a => a.Email == email);
            if(existingUser)
            {
                throw new UserEntityAlreadyExistsException();
            }

            var passwordHash = PasswordHash.CreateHash(password);

            var user = new PenedatingMongoUser()
                           {
                               Email = email,
                               PasswordHash = passwordHash
                           };

            var result = _mongoCollection.Insert(user);
            if(result.Ok)
            {
                return Login(email, password);
            }

            throw new Exception("Something went wrong. Amagad");
        }

        public User GetUserByID(string userId)
        {
            var query = _mongoCollection.AsQueryable();
            var mongoUser = query.SingleOrDefault(a => a.UserID == new BsonObjectId(userId));

            if(mongoUser == null)
            {
                throw new UserEntityNotFoundException();
            }

            return new User
                       {
                           UserID = mongoUser.UserID.ToString(),
                           Email = mongoUser.Email
                       };
        }

        public UserProfile GetUserProfile(string userId)
        {
            var query = _mongoCollection.AsQueryable();
            var mongoUser = query.SingleOrDefault(a => a.UserID == new BsonObjectId(userId));

            if (mongoUser == null)
            {
                throw new UserEntityNotFoundException();
            }

            return mongoUser.UserProfile;
        }

        public void UpdateProfile(string userId, UserProfile profile)
        {
            var query = _mongoCollection.AsQueryable();
            var user = query.SingleOrDefault(a => a.UserID == new BsonObjectId(userId));
            if (user == null)
            {
                throw new UserEntityNotFoundException();
            }

            user.UserProfile = profile;
            _mongoCollection.Save(user);
        }
    }
}