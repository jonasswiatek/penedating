using System.Collections.Generic;

namespace Penedating.Web.Models
{
    public class ExternalProfile
    {
        public string name { get; set; }
        public IEnumerable<string> hobbies { get; set; }
        public string url { get; set; }
    }
}
