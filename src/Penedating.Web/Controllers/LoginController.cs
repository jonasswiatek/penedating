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

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel loginModel)
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

                Session[MvcApplication.UserStateCookieName] = userState;
            
                return RedirectToAction("Index", "Home");
            }
            catch(InvalidUserCredentialsException iuce)
            {
                ModelState.AddModelError("email", "Invalid email or password");

                return View(loginModel);
            }
        }
    }
}
