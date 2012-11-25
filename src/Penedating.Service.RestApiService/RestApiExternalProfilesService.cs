using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;

namespace Penedating.Service.RestApiService
{
    public class RestApiExternalProfilesService : IExternalProfilesService
    {
        private readonly IEnumerable<Uri> _partners;

        public RestApiExternalProfilesService(IEnumerable<Uri> partners)
        {
            _partners = partners;
        }

        public IEnumerable<PartnerQueryResult> GetExternalProfiles()
        {
            var tasks = _partners.Select(a => Task.Factory.StartNew(() =>
                                                                        {
                                                                            {
                                                                                using (var client = new WebClient())
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        client.Headers["X-Limit"] = "10";
                                                                                        var jsonString = client.DownloadString(a);
                                                                                        var result = JsonConvert.DeserializeObject<ExternalProfile[]>(jsonString);

                                                                                        return new PartnerQueryResult()
                                                                                                   {
                                                                                                       PartnerUri = a,
                                                                                                       Profiles = result
                                                                                                   };
                                                                                    }
                                                                                    catch (Exception e)
                                                                                    {
                                                                                        return new PartnerQueryResult()
                                                                                                   {
                                                                                                       PartnerUri = a,
                                                                                                       ExceptionThrown = e
                                                                                                   };
                                                                                    }
                                                                                }
                                                                            }
                                                                        })).ToArray();

            Task.WaitAll(tasks);
            var results = tasks.Select(a => a.Result);

            return results;
        }
    }
}