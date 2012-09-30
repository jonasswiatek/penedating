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

namespace Penedating.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginModel);
            }

            try
            {
                var userCredentials = Mapper.Map<UserCredentials>(loginModel);

                //Attempt to login the user. This method will throw an exception if this fails.
                var accessToken = _userService.Login(userCredentials);

                //Please notice that we do not store the user object it self. This is to remain scalable.
                var userState = new UserState   
                                    {
                                        Email = userCredentials.Email,
                                        AccessToken = accessToken
                                    };

                Session["UserState"] = userState;
            
                return RedirectToAction("Index", "Home");
            }
            catch(InvalidUserCredentialsException iuce)
            {
                ModelState.AddModelError("Username", "Invalid");
                ModelState.AddModelError("Password", "Invalid");

                return View(loginModel);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserCreateModel userCreateModel)
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
                ModelState.AddModelError("Username", "Exists");
                return View(userCreateModel);
            }

            var userProfile = Mapper.Map<UserProfile>(userCreateModel);

            _userService.UpdateProfile(accessToken, userProfile);

            //Login the newly created user
            return Login(userCreateModel);
        }
    }
}