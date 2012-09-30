using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Penedating.Data.MongoDB.Model
{
    public class User
    {
        public string UserID;
        public string Username;
        public Address Address;
    }
}
