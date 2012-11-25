using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penedating.Service.Model.Contract
{
    public interface IExternalProfilesService
    {
        IEnumerable<PartnerQueryResult> GetExternalProfiles();
    }
}