using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Service.Model.Exceptions;
using Penedating.Web.Models;
using Penedating.Web.Security;

namespace Penedating.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserProfileService _userProfileService;

        public UserController(IUserService userService, IUserProfileService userProfileService)
        {
            _userService = userService;
            _userProfileService = userProfileService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(CreateViewModel createViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(createViewModel);
            }

            UserAccessToken accessToken;
            try
            {
                var userCredentials = Mapper.Map<UserCredentials>(createViewModel);
                accessToken = _userService.Create(userCredentials);
            }
            catch(UserExistsException uee)
            {
                ModelState.AddModelError("email", "Email already in use");
                return View(createViewModel);
            }

            var userProfile = Mapper.Map<UserProfile>(createViewModel);

            _userProfileService.UpdateProfile(accessToken, userProfile);

            //Login the newly created user
            return RedirectToAction("CreatedConfirm");
        }

        public ActionResult CreatedConfirm()
        {
            return View();
        }
    }
}