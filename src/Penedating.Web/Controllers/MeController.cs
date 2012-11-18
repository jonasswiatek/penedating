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
        private readonly IUserProfileService _userProfileService;
        private readonly IUserAccessTokenProvider _accessTokenProvider;

        public MeController(IUserService userService, IUserProfileService userProfileService, IUserAccessTokenProvider accessTokenProvider)
        {
            _userService = userService;
            _userProfileService = userProfileService;
            _accessTokenProvider = accessTokenProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            var accessToken = _accessTokenProvider.GetAccessToken();
            var userProfile = _userProfileService.GetUserProfile(accessToken);
            var profileViewModel = Mapper.Map<ProfileViewModel>(userProfile);

            return View(profileViewModel);
        }

        [HttpPost]
        public ActionResult Profile(ProfileViewModel profileViewModel)
        {
            var accessToken = _accessTokenProvider.GetAccessToken();
            var userProfile = _userProfileService.GetUserProfile(accessToken);

            if(!ModelState.IsValid)
            {
                profileViewModel.Hobbies = userProfile.Hobbies;
                return View(profileViewModel);
            }

            //TODO: This is just inefficient. The service layer and repository design needs to support spot updating stuff like this much better.
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

            _userProfileService.UpdateProfile(accessToken, newUserProfile);

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public ActionResult Hobby(string hobby, bool remove)
        {
            var accessToken = _accessTokenProvider.GetAccessToken();
            var profile = _userProfileService.GetUserProfile(accessToken);

            //TODO: Make this transport in a view model and support client errors properly. While this works it's not in line with the rest of the app.
            if(string.IsNullOrEmpty(hobby) || hobby.Length > 20)
            {
                return View("Hobbies", profile.Hobbies);
            }

            if(remove)
            {
                profile.Hobbies.Remove(hobby);
            }
            else
            {
                profile.Hobbies.Add(hobby);
            }

            _userProfileService.UpdateProfile(accessToken, profile);
            return View("Hobbies", profile.Hobbies);
        }

        public ActionResult Logout()
        {
            _accessTokenProvider.DestroyAccessToken();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteAccount(string auth)
        {
            var accessToken = _accessTokenProvider.GetAccessToken();
            if (accessToken.Ticket != auth || accessToken.IsAdmin) //I don't want to have to repromote admins all the time, so they can't delete them selves.
            {
                return RedirectToAction("Index");
            }

            _userService.DeleteUser(accessToken);
            _accessTokenProvider.DestroyAccessToken();

            return Logout();
        }
    }
}
