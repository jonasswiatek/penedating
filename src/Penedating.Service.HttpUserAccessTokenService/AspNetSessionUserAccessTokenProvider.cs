using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Service.Model.Exceptions;

namespace Penedating.Service.HttpUserAccessTokenService
{
    public class AspNetSessionUserAccessTokenProvider : IUserAccessTokenProvider
    {
        private readonly string _sessionKeyName;
        private readonly HttpContext _httpContext;

        public AspNetSessionUserAccessTokenProvider(string sessionKeyName)
        {
            _sessionKeyName = sessionKeyName;
            _httpContext = HttpContext.Current;
        }

        public void SetUserAccessToken(UserAccessToken accessToken)
        {
            _httpContext.Session[_sessionKeyName] = accessToken;
        }

        public UserAccessToken GetAccessToken()
        {
            var accessToken = _httpContext.Session[_sessionKeyName] as UserAccessToken;
            if (accessToken == null)
            {
                throw new UserAccessTokenNotFoundException();
            }

            return accessToken;
        }

        public bool TryGetAccessToken(out UserAccessToken userAccessToken)
        {
            var accessToken = _httpContext.Session[_sessionKeyName] as UserAccessToken;
            userAccessToken = accessToken;

            return accessToken != null;
        }

        public void DestroyAccessToken()
        {
            _httpContext.Session.Abandon();
        }
    }
}