using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Penedating.Web.Security;

namespace Penedating.Web.Controllers
{
    [LoginRequired]
    public class MeController : Controller
    {
        //
        // GET: /Me/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }
    }
}
