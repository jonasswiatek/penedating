using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model
{
    public class PartnerQueryResult
    {
        public Uri PartnerUri;
        public IEnumerable<ExternalProfile> Profiles = new List<ExternalProfile>();
        public string PartnerError;
    }
}
