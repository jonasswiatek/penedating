using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Penedating.Web.Models;

namespace Penedating.Web.Security
{
    public class LoginRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userState = filterContext.HttpContext.Session[MvcApplication.UserStateCookieName] as UserState;
            if(userState == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                                                                                              {
                                                                                                  controller = "User",
                                                                                                  action = "Login"
                                                                                              }));
            }
        }
    }
}