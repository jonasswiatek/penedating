using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Data.MongoDB.Model.Contract
{
    public interface IHugRepository
    {
        void InsertHug(string recipient, Hug hug);
        IEnumerable<Hug> GetHugs(string userId);
        void DismissHugs(string userId);
    }
}
