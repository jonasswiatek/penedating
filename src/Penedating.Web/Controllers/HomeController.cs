using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Penedating.Web.Security;

namespace Penedating.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(UserState.IsAvailable)
            {
                return RedirectToAction("Index", "Me");
            }
            return View();
        }
    }
}