using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model.Contract
{
    public interface IHugService
    {
        IEnumerable<Hug> GetHugs(UserAccessToken userAccessToken);
        void SendHug(UserAccessToken userAccessToken, string recipientUserId);
        void DismissHugs(UserAccessToken userAccessToken);
    }
}