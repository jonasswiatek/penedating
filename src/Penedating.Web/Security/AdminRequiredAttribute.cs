using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Web.Models;

namespace Penedating.Web.Security
{
    public class AdminRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var accessTokenProvider = DependencyResolver.Current.GetService<IUserAccessTokenProvider>();

            UserAccessToken userAccessToken;
            if(accessTokenProvider.TryGetAccessToken(out userAccessToken))
            {
                if (userAccessToken.IsAdmin)
                {
                    //The user is an administrator according to his access token.
                    return;
                }
            }
            /* Throw an HttpException to forcibly destroy the ASP.NET Request Pipe Line.
             * This has the added advantage of showing a proper 404 page from the web server. */
            throw new HttpException(404, "Not found");
        }
    }
}