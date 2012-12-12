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
        private readonly IExternalProfilesService _externalProfilesService;

        public ProfilesController(IUserProfileService userProfileService, IExternalProfilesService externalProfilesService)
        {
            _userProfileService = userProfileService;
            _externalProfilesService = externalProfilesService;
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

        public ActionResult ExternalProfiles(int pageIndex = 0, int pageSize = 4)
        {
            var profiles = _externalProfilesService.GetExternalProfiles();

            var profileListItems = profiles.SelectMany(a => a.Profiles);
            var pageCount = ((int)profileListItems.Count()) / pageSize;
            if (profileListItems.Count() % pageSize != 0)
            {
                pageCount += 1;
            }

            var profileItems = Mapper.Map<IEnumerable<ProfileListItem>>(profileListItems);

            var viewModel = new ProfileList()
            {
                PageCount = pageCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Profiles = profileItems.Skip(pageIndex*pageSize).Take(pageSize)
            };

            return View("Index", viewModel);
        }
    }
}
