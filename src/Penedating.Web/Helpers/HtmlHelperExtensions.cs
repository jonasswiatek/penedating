using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Service.Model.Exceptions;

namespace Penedating.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static bool IsUserLoggedIn(this HtmlHelper htmlHelper)
        {
            var accessTokenProvider = DependencyResolver.Current.GetService<IUserAccessTokenProvider>();
            UserAccessToken accessToken;
            return accessTokenProvider.TryGetAccessToken(out accessToken);
        }

        public static string GetUserEmail(this HtmlHelper htmlHelper)
        {
            var accessTokenProvider = DependencyResolver.Current.GetService<IUserAccessTokenProvider>();
            UserAccessToken accessToken;
            if(accessTokenProvider.TryGetAccessToken(out accessToken))
            {
                return accessToken.Email;
            }

            throw new UserAccessTokenNotFoundException();
        }
    }
}