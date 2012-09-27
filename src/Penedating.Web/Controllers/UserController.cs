using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Penedating.Web.Models;

namespace Penedating.Web.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult New()
        {
            return View();
        }
    }
}