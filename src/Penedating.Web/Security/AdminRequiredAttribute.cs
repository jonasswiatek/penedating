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

            if(!accessTokenProvider.TryGetAccessToken(out userAccessToken))
            {
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            if(!userAccessToken.IsAdmin)
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}