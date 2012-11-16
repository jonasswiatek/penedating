using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model
{
    public class MultipageResponse<T> where T : class
    {
        public IEnumerable<T> Result;
        public int PageSize;
        public int PageIndex;
        public int PageCount;
    }
}
