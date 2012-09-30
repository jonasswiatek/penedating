using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Penedating.Data.MongoDB.Model
{
    public class MongoUser
    {
        [BsonId]
        public string UserID { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}