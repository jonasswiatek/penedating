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
    public class LoginRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var accessTokenProvider = DependencyResolver.Current.GetService<IUserAccessTokenProvider>();
            UserAccessToken userAccessToken;

            if(!accessTokenProvider.TryGetAccessToken(out userAccessToken))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                                                                                              {
                                                                                                  controller = "Login",
                                                                                                  action = "Index"
                                                                                              }));
            }
        }
    }
}