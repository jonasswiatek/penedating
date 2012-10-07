using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model
{
    public class UserAccessToken
    {
        public string Ticket { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public UserAccessToken()
        {
        }

        public UserAccessToken(string ticket)
        {
            Ticket = ticket;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserAccessToken);
        }

        public bool Equals(UserAccessToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Ticket, Ticket);
        }

        public override int GetHashCode()
        {
            return (Ticket != null ? Ticket.GetHashCode() : 0);
        }
    }
}
