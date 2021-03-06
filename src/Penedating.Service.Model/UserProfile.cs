﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model
{
    public class UserProfile
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public Address Address { get; set; }
        public IList<string> Hobbies { get; set; }
        public IList<Interest> Interests { get; set; } 

        public UserProfile()
        {
            Hobbies = new List<string>();
            Interests = new List<Interest>();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserProfile);
        }

        public bool Equals(UserProfile other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Username, Username) && Equals(other.Address, Address);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Username != null ? Username.GetHashCode() : 0)*397) ^ (Address != null ? Address.GetHashCode() : 0);
            }
        }
    }
}