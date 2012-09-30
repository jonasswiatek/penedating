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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserCreateModel userCreateModel)
        {
            if(!ModelState.IsValid)
            {
                return View(userCreateModel);
            }

            UserAccessToken accessToken;
            try
            {
                var userCredentials = Mapper.Map<UserCredentials>(userCreateModel);
                accessToken = _userService.Create(userCredentials);
            }
            catch(UserExistsException uee)
            {
                ModelState.AddModelError("email", "Email already in use");
                return View(userCreateModel);
            }

            var userProfile = Mapper.Map<UserProfile>(userCreateModel);

            _userService.UpdateProfile(accessToken, userProfile);

            //Login the newly created user
            return RedirectToAction("CreatedConfirm");
        }

        public ActionResult CreatedConfirm()
        {
            return View();
        }
    }
}