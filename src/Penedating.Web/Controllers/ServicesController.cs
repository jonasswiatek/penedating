using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Web.Models;

namespace Penedating.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IUserProfileService _userProfileService;

        public ServicesController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [OutputCache(Duration = 10, Location = OutputCacheLocation.Any)]
        public ActionResult Users()
        {
            var limitHeaderValue = ControllerContext.HttpContext.Request.Headers["X-Limit"];
            var limit = 10;
            if (!string.IsNullOrEmpty(limitHeaderValue))
            {
                int.TryParse(limitHeaderValue, out limit);
            }

            var profiles = _userProfileService.GetProfiles(0, limit);
            var profilesModel = Mapper.Map<IEnumerable<ExternalProfile>>(profiles.Result);

            return Json(profilesModel, JsonRequestBehavior.AllowGet);
        }
    }
}