using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Penedating.Data.MongoDB.Model;
using Penedating.Data.MongoDB.Model.Contract;
using Penedating.Data.MongoDB.Model.Exceptions;

namespace Penedating.Data.MongoDB
{
    public class MongoUserRepository : IUserRepository
    {
        private MongoCollection<Model.MongoUser> _mongoCollection;
        private MongoDatabase _database;
        private readonly MongoServer _server;

        public MongoUserRepository(string connectionString, string databaseName)
        {
            _server = MongoServer.Create(connectionString);
            _database = _server.GetDatabase(databaseName);
            _mongoCollection = _database.GetCollection<Model.MongoUser>("users");
        }

        public User Login(string username, byte[] passwordHash)
        {
            var query = _mongoCollection.AsQueryable();
            var mongoUser = query.SingleOrDefault(a => a.Email == username && a.PasswordHash == passwordHash);
            if(mongoUser == null)
            {
                throw new UserEntityNotFoundException();
            }

            return new User()
                       {
                           UserID = mongoUser.UserID,
                           Email = mongoUser.Email
                       };
        }

        public User Create(string email, byte[] passwordHash)
        {
            var query = _mongoCollection.AsQueryable();
            var existingUser = query.Any(a => a.Email == email);
            if(existingUser)
            {
                throw new UserEntityAlreadyExistsException();
            }

            var user = new Model.MongoUser()
                           {
                               Email = email,
                               PasswordHash = passwordHash
                           };

            var result = _mongoCollection.Insert(user);
            if(result.Ok)
            {
                return Login(email, passwordHash);
            }

            throw new Exception("Something went wrong. Amagad");
        }

        public User GetUserByID(string userId)
        {
            var query = _mongoCollection.AsQueryable();
            var mongoUser = query.SingleOrDefault(a => a.UserID == userId);

            if(mongoUser == null)
            {
                throw new UserEntityNotFoundException();
            }

            return new User
                       {
                           Email = mongoUser.Email,
                           UserID = mongoUser.UserID
                       };
        }
    }
}