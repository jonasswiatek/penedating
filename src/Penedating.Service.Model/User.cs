using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model
{
    public class User
    {
        public string UserID { get; private set; }
        public string Name { get; private set; }
        public Address Address { get; set; }
        public IEnumerable<Hobby> Hobbies { get; set; }
        public IEnumerable<Interest> Interests { get; set; }

        public User()
        {
        }

        public User(string userId, string name)
        {
            UserID = userId;
            Name = name;
        }
    }
}