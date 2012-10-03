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
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserAccessTokenProvider _userAccessTokenProvider;

        public LoginController(IUserService userService, IUserAccessTokenProvider userAccessTokenProvider)
        {
            _userService = userService;
            _userAccessTokenProvider = userAccessTokenProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            try
            {
                var userCredentials = Mapper.Map<UserCredentials>(loginViewModel);

                //Attempt to login the user. This method will throw an exception if this fails.
                var accessToken = _userService.Login(userCredentials);
                _userAccessTokenProvider.SetUserAccessToken(accessToken);
            
                return RedirectToAction("Index", "Home");
            }
            catch(InvalidUserCredentialsException iuce)
            {
                ModelState.AddModelError("email", "Invalid email or password");

                return View(loginViewModel);
            }
        }
    }
}