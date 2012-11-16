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
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly MongoCollection<PenedatingMongoUser> _mongoCollection;
        private readonly MongoDatabase _database;
        private readonly MongoServer _server;

        public UserProfileRepository(string connectionString, string databaseName)
        {
            _server = MongoServer.Create(connectionString);
            _database = _server.GetDatabase(databaseName);
            _mongoCollection = _database.GetCollection<PenedatingMongoUser>("users");
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
            var query = Query.EQ("_id", new BsonObjectId(userId));
            var update = Update.Set("UserProfile", profile.ToBsonDocument());
            _mongoCollection.FindAndModify(query, SortBy.Null, update);
        }

        public IEnumerable<UserProfile> GetProfiles(int pageIndex, int pageSize, out int pageCount)
        {
            var query = _mongoCollection.FindAll();
            query.SetLimit(pageSize);
            query.SetSkip(pageSize*pageIndex);
            pageCount = ((int)query.Count()) / pageSize;

            return query.Select(a => a.UserProfile).ToList();
        }
    }
}