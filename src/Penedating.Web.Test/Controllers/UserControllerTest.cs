using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Penedating.Web.Controllers;
using Penedating.Web.Models;

namespace Penedating.Web.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest
    {
        [Test]
        public void TestCreate_TestModelError()
        {
            var userController = new UserController();
            userController.ViewData.ModelState.AddModelError("rofl", "nao"); //This simulates any validation error
            var loginModel = new LoginModel();
            
            var result = userController.Login(loginModel) as ViewResult;
            Assert.IsNotNull(result, "Login Action did not yield a ViewResult");
            Assert.AreSame(loginModel, result.Model, "Controller did not forward the defective view");
        }

        [Test]
        public void TestCreate_Success()
        {
            var userController = new UserController();
            var loginModel = new LoginModel();

            var result = userController.Login(loginModel) as RedirectToRouteResult;
            Assert.IsNotNull(result, "Login Action did not yield a Redirection");
            Assert.AreEqual(result.RouteValues["controller"], "Home");
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }
    }
}