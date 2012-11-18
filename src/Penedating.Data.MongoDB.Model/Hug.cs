using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Penedating.Data.MongoDB.Model
{
    public class Hug
    {
        public string SenderID { get; set; }
        [BsonIgnore]
        public UserProfile Sender { get; set; }
        public DateTime Created { get; set; }
    }
}