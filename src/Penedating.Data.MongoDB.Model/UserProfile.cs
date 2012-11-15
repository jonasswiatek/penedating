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

        public IList<string> Hobbies { get; set; }
        public IList<string> Interests { get; set; }

        public UserProfile()
        {
            Hobbies = new List<string>();
            Interests = new List<string>();
        }
    }
}