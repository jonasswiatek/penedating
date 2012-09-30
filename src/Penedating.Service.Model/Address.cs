using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Address);
        }

        public bool Equals(Address other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Street, Street) && Equals(other.City, City) && other.ZipCode == ZipCode;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Street != null ? Street.GetHashCode() : 0);
                result = (result*397) ^ (City != null ? City.GetHashCode() : 0);
                result = (result*397) ^ ZipCode;
                return result;
            }
        }
    }
}
