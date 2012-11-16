using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Penedating.Service.Model.Contract;
using Penedating.Web.Models;

namespace Penedating.Web.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly IUserProfileService _userProfileService;

        public ProfilesController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        public ActionResult Index(int pageIndex = 0, int pageSize = 4)
        {
            var profiles = _userProfileService.GetProfiles(pageIndex, pageSize);
            var profileListItems = Mapper.Map<IEnumerable<ProfileListItem>>(profiles.Result);

            var viewModel = new ProfileList()
            {
                PageCount = profiles.PageCount,
                PageSize = profiles.PageSize,
                PageIndex = profiles.PageIndex,
                Profiles = profileListItems
            };

            return View(viewModel);
        }
    }
}
