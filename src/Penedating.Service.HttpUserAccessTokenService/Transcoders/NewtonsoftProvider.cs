using System.Collections.Generic;
using Enyim.Caching.Memcached;

namespace Penedating.Service.HttpUserAccessTokenService.Transcoders
{
    public class NewtonsoftProvider : IProviderFactory<ITranscoder>
    {
        public ITranscoder Create()
        {
            return new NewtonsoftTranscoder();
        }

        public void Initialize(Dictionary<string, string> parameters)
        {
            //I have no idea what this does.
        }
    }
}
