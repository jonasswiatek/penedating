using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Data.MongoDB.Model
{
    public class User
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}