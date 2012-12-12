using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Penedating.Web.Models
{
    public class ProfileList
    {
        public int PageIndex;
        public int PageCount;
        public int PageSize;

        public IEnumerable<ProfileListItem> Profiles;
        public string randId;
    }
}