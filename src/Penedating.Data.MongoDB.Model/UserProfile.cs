using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Penedating.Data.MongoDB.Model
{
    public class UserProfile
    {
        [BsonIgnore]
        public string MongoUserID { get; set; }

        public string Username { get; set; }
        public Address Address { get; set; }

        public IList<string> Hobbies { get; set; }
        public IList<string> Interests { get; set; }

        public UserProfile()
        {
            Hobbies = new List<string>();
            Interests = new List<string>();
        }
    }
}