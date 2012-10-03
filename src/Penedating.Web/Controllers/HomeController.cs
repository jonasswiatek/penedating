using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Web.Security;

namespace Penedating.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserAccessTokenProvider _accessTokenProvider;

        public HomeController(IUserAccessTokenProvider accessTokenProvider)
        {
            _accessTokenProvider = accessTokenProvider;
        }

        public ActionResult Index()
        {
            UserAccessToken accessToken;
            if (_accessTokenProvider.TryGetAccessToken(out accessToken))
            {
                return RedirectToAction("Index", "Me");
            }

            return View();
        }
    }
}