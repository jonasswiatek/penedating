using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
                                        try
                                        {
                                            var request = (HttpWebRequest) WebRequest.Create(a);
                                            request.Timeout = 12000;
                                            request.Headers["X-Limit"] = "10";

                                            using (var response = request.GetResponse())
                                            using (var responseStream = response.GetResponseStream())
                                            using (var streamReader = new StreamReader(responseStream))
                                            {
                                                var jsonText = streamReader.ReadToEnd();
                                                var result = JsonConvert.DeserializeObject<ExternalProfile[]>(jsonText);

                                                if (result.Count() > 10)
                                                {
                                                    throw new Exception("Partner: " + a + " didn't not respect the X-Limit header");
                                                }

                                                return new PartnerQueryResult
                                                            {
                                                                PartnerUri = a,
                                                                Profiles = result
                                                            };
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            return new PartnerQueryResult
                                                        {
                                                            PartnerUri = a,
                                                            PartnerError = e.Message
                                                        };
                                        }
                                    }
                                    )).ToArray();

            Task.WaitAll(tasks);
            var results = tasks.Select(a => a.Result);

            return results;
        }
    }
}