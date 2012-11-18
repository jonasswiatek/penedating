using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Penedating.Web.Models
{
    public class ProfileListItem
    {
        public string Username;
        public string UserID;
        public IEnumerable<string> Hobbies { get; set; }
        public bool Friendship { get; set; }
        public bool Romance { get; set; }
    }
}