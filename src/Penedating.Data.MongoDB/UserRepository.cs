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

        public User Login(string email, string password)
        {
            var query = _mongoCollection.AsQueryable();
            var mongoUser = query.SingleOrDefault(a => a.Email == email);
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
                           Email = mongoUser.Email,
                           IsAdmin = mongoUser.IsAdmin
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

        public void DeleteUser(string userId)
        {
            _mongoCollection.Remove(Query.EQ("_id", new BsonObjectId(userId)));
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
                           /* We explicitly ignore the IsAdmin field here. */
                       };
        }

        public IEnumerable<User> GetAllUsers()
        {
            var query = _mongoCollection.AsQueryable();
            return query.Select(a => new User()
                                         {
                                             Email = a.Email,
                                             UserID = a.UserID.ToString()
                                         });
        }
    }
}