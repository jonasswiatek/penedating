using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Web.Models;
using Penedating.Web.Security;

namespace Penedating.Web.Controllers
{
    [LoginRequired]
    public class MeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserAccessTokenProvider _accessTokenProvider;

        public MeController(IUserService userService, IUserAccessTokenProvider accessTokenProvider)
        {
            _userService = userService;
            _accessTokenProvider = accessTokenProvider;
        }

        public ActionResult Index()
        {
            var accessToken = _accessTokenProvider.GetAccessToken();
            var userProfile = _userService.GetUserProfile(accessToken);
            var profileViewModel = Mapper.Map<ProfileViewModel>(userProfile);

            return View(profileViewModel);
        }

        [HttpPost]
        public ActionResult Index(ProfileViewModel profileViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(profileViewModel);
            }

            var userProfile = Mapper.Map<UserProfile>(profileViewModel);
            var accessToken = _accessTokenProvider.GetAccessToken();

            _userService.UpdateProfile(accessToken, userProfile);

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            _accessTokenProvider.DestroyAccessToken();

            return RedirectToAction("Index", "Home");
        }
    }
}
