using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model
{
    public class UserCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserCredentials);
        }

        public bool Equals(UserCredentials other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Email, Email) && Equals(other.Password, Password);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Email != null ? Email.GetHashCode() : 0)*397) ^ (Password != null ? Password.GetHashCode() : 0);
            }
        }
    }
}