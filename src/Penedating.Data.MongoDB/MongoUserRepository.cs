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
        private MongoCollection<User> _mongoCollection;
        private MongoDatabase _database;
        private readonly MongoServer _server;

        public MongoUserRepository(string connectionString, string databaseName)
        {
            _server = MongoServer.Create(connectionString);
            _database = _server.GetDatabase(databaseName);
            _mongoCollection = _database.GetCollection<User>("users");
        }

        public User Login(string username, byte[] passwordHash)
        {
            var query = _mongoCollection.AsQueryable();
            var user = query.SingleOrDefault(a => a.Username == username);
            if(user == null)
            {
                throw new UserEntityNotFoundException();
            }

            return user;
        }

        public User Create(string username, byte[] passwordHash)
        {
            var query = _mongoCollection.AsQueryable();
            var existingUser = query.Any(a => a.Username == username);
            if(existingUser)
            {
                throw new UserEntityAlreadyExistsException();
            }

            var user = new User()
                           {
                               Username = username
                           };

            var result = _mongoCollection.Insert(user);
            if(result.Ok)
            {
                return Login(username, passwordHash);
            }

            throw new Exception("Something went wrong. Amagad");
        }

        public User Update(User user)
        {
            var query = _mongoCollection.AsQueryable();
            return null;
        }

        public User GetUserByID(string userId)
        {
            throw new NotImplementedException();
        }
    }
}