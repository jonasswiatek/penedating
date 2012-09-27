using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model
{
    public class User
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public IEnumerable<Hobby> Hobbies { get; set; }
        public IEnumerable<Interest> Interests { get; set; }
    }
}