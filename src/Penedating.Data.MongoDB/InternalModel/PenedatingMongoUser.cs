using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Penedating.Data.MongoDB.Model;

namespace Penedating.Data.MongoDB.InternalModel
{
    internal class PenedatingMongoUser
    {
        [BsonId]
        public BsonObjectId UserID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}