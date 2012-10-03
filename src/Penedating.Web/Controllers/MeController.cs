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

            //TODO: This is just inefficient. The service layer and repository design needs to support spot updating stuff like this much better.
            var accessToken = _accessTokenProvider.GetAccessToken();
            var userProfile = _userService.GetUserProfile(accessToken);
            var newUserProfile = Mapper.Map<UserProfile>(profileViewModel);
            newUserProfile.Hobbies = userProfile.Hobbies;
            newUserProfile.Interests.Clear();

            if(profileViewModel.Friendship)
            {
                newUserProfile.Interests.Add(Interest.Friendship);
            }

            if (profileViewModel.Romance)
            {
                newUserProfile.Interests.Add(Interest.Romance);
            }

            _userService.UpdateProfile(accessToken, newUserProfile);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Hobby(string hobby, bool remove)
        {
            var accessToken = _accessTokenProvider.GetAccessToken();
            var profile = _userService.GetUserProfile(accessToken);

            if(remove)
            {
                profile.Hobbies.Remove(hobby);
            }
            else
            {
                profile.Hobbies.Add(hobby);
            }
            
            _userService.UpdateProfile(accessToken, profile);
            return View("Hobbies", profile.Hobbies);
        }

        public ActionResult Logout()
        {
            _accessTokenProvider.DestroyAccessToken();

            return RedirectToAction("Index", "Home");
        }
    }
}
