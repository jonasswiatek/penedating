using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Penedating.Data.MongoDB.Model;
using Penedating.Data.MongoDB.Model.Contract;
using Penedating.Data.MongoDB.Model.Exceptions;
using MongoUser = Penedating.Data.MongoDB.InternalModel.MongoUser;

namespace Penedating.Data.MongoDB
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoCollection<MongoUser> _mongoCollection;
        private readonly MongoDatabase _database;
        private readonly MongoServer _server;

        public UserRepository(string connectionString, string databaseName)
        {
            _server = MongoServer.Create(connectionString);
            _database = _server.GetDatabase(databaseName);
            _mongoCollection = _database.GetCollection<MongoUser>("users");
        }

        public User Login(string username, string password)
        {
            var query = _mongoCollection.AsQueryable();
            var mongoUser = query.SingleOrDefault(a => a.Email == username);
            if(mongoUser == null)
            {
                throw new UserEntityNotFoundException();
            }

            var salt = mongoUser.PasswordSalt;
            var hash = GenerateSaltedHash(password, salt);

            if(!hash.SequenceEqual(mongoUser.PasswordHash))
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

            var passwordSalt = GenerateSalt();
            var passwordHash = GenerateSaltedHash(password, passwordSalt);

            var user = new MongoUser()
                           {
                               Email = email,
                               PasswordSalt = passwordSalt,
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
            var mongoUser = query.SingleOrDefault(a => a.UserID == userId);

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

        private static byte[] GenerateSaltedHash(string plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            var password = Encoding.UTF8.GetBytes(plainText);

            var salted = password.Concat(salt).ToArray();
            return algorithm.ComputeHash(salted);
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[25];

            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);

            return salt;
        }
    }
}