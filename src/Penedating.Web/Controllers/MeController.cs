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

        public MeController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            var userState = UserState.Current;
            var userProfile = _userService.GetUserProfile(userState.AccessToken);
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
            _userService.UpdateProfile(UserState.Current.AccessToken, userProfile);

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }
    }
}
