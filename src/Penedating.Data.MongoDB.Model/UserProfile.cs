using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Data.MongoDB.Model
{
    public class UserProfile
    {
        public string Username { get; set; }
        public Address Address { get; set; }
    }
}
