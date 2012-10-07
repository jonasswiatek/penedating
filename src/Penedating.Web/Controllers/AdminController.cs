using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Penedating.Service.Model.Contract;
using Penedating.Web.Models;
using Penedating.Web.Security;

namespace Penedating.Web.Controllers
{
    [AdminRequired]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserAccessTokenProvider _userAccessTokenProvider;

        public AdminController(IUserService userService, IUserAccessTokenProvider userAccessTokenProvider)
        {
            _userService = userService;
            _userAccessTokenProvider = userAccessTokenProvider;
        }

        public ActionResult Index()
        {
            var users = _userService.GetUsers().Select(a => new AdminUserProfile
                                                                  {
                                                                      Email = a.Email,
                                                                      UserID = a.Ticket
                                                                  });
            return View(new AdminViewModel()
                            {
                                Users = users
                            });
        }

        public ActionResult Impersonate(string ticket)
        {
            //Terminate existing user session
            _userAccessTokenProvider.DestroyAccessToken();

            var newUser = _userService.ImpersonateUser(ticket);
            _userAccessTokenProvider.SetUserAccessToken(newUser);

            return RedirectToAction("Index", "Me");
        }
    }
}