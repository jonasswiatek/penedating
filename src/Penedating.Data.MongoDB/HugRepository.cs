using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Penedating.Data.MongoDB.InternalModel;
using Penedating.Data.MongoDB.Model;
using Penedating.Data.MongoDB.Model.Contract;

namespace Penedating.Data.MongoDB
{
    public class HugRepository : IHugRepository
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly MongoCollection<PenedatingMongoUser> _mongoCollection;
        private readonly MongoDatabase _database;
        private readonly MongoServer _server;

        public HugRepository(string connectionString, string databaseName, IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
            _server = MongoServer.Create(connectionString);
            _database = _server.GetDatabase(databaseName);
            _mongoCollection = _database.GetCollection<PenedatingMongoUser>("users");
        }

        public void InsertHug(string recipient, Hug hug)
        {
            var mongUserObject = _mongoCollection.AsQueryable().Single(a => a.UserID == new BsonObjectId(recipient));
            var hugs = mongUserObject.Hugs;

            if(hugs.Any(a => a.SenderID == hug.SenderID)) //No reason to store this - the user has already hugged.
                return;

            hugs.Add(hug);

            var query = Query.EQ("_id", new BsonObjectId(recipient));
            var update = Update.AddToSet("Hugs", hug.ToBsonDocument());
            _mongoCollection.FindAndModify(query, SortBy.Null, update);
        }

        public IEnumerable<Hug> GetHugs(string userId)
        {
            var mongUserObject = _mongoCollection.AsQueryable().Single(a => a.UserID == new BsonObjectId(userId));
            var hugs = mongUserObject.Hugs.ToList();
            foreach (var hug in hugs)
            {
                hug.Sender = _userProfileRepository.GetUserProfile(hug.SenderID);
            }

            return hugs;
        }

        public void DismissHugs(string userId)
        {
            var query = Query.EQ("_id", new BsonObjectId(userId));
            var update = Update.Unset("Hugs");
            _mongoCollection.FindAndModify(query, SortBy.Null, update);
        }
    }
}