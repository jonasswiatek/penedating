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
        private readonly IExternalProfilesService _externalProfilesService;

        public HomeController(IUserAccessTokenProvider accessTokenProvider, IExternalProfilesService externalProfilesService)
        {
            _accessTokenProvider = accessTokenProvider;
            _externalProfilesService = externalProfilesService;
        }

        public ActionResult Index()
        {
            //var externalUsers = _externalProfilesService.GetExternalProfiles();

            UserAccessToken accessToken;
            if (_accessTokenProvider.TryGetAccessToken(out accessToken))
            {
                return RedirectToAction("Index", "Me");
            }

            return View();
        }
    }
}