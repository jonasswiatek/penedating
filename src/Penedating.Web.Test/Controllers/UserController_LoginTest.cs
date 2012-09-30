using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Moq;
using NUnit.Framework;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Service.Model.Exceptions;
using Penedating.Web.Controllers;
using Penedating.Web.Models;

namespace Penedating.Web.Test.Controllers
{
    [TestFixture]
    public class UserController_LoginTest
    {
        [Test]
        public void Login_TestFormValidation()
        {
            var userServiceMock = new Mock<IUserService>();
            var userService = userServiceMock.Object;

            var userController = new UserController(userService);
            userController.ViewData.ModelState.AddModelError("rofl", "nao"); //This simulates any validation error
            var loginModel = new LoginModel();
            var credentials = new UserCredentials()
                                  {
                                      Email = loginModel.Email,
                                      Password = loginModel.Password
                                  };

            var result = userController.Login(loginModel) as ViewResult;

            //Verify that the controller does not try to call login on IUserService
            userServiceMock.Verify(a => a.Login(credentials), Times.Never());

            Assert.IsNotNull(result, "Login Action did not yield a ViewResult");
            Assert.AreSame(loginModel, result.Model, "Controller did not forward the defective view");
        }

        [Test]
        public void Login_InvalidCredentials()
        {
            var userServiceMock = new Mock<IUserService>();
            var userService = userServiceMock.Object;

            var userController = new UserController(userService);

            var loginModel = new LoginModel()
            {
                Email = "testuser",
                Password = "testpasswordrofl"
            };

            var credentials = new UserCredentials()
                                  {
                                      Email = loginModel.Email,
                                      Password = loginModel.Password
                                  };

            userServiceMock.Setup(a => a.Login(credentials)).Throws(new InvalidUserCredentialsException());

            var actionResult = userController.Login(loginModel) as ViewResult;

            Assert.IsNotNull(actionResult, "Controller did not return a ViewResult");
            Assert.AreSame(loginModel, actionResult.Model, "Controller did not forward the defective view");
        }

        [Test]
        public void Login_Success()
        {
            var userServiceMock = new Mock<IUserService>();
            var userService = userServiceMock.Object;

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupSet(a => a.HttpContext.Session["UserState"] = It.IsAny<UserState>()).Verifiable("Controllre did not set session state");

            var userController = new UserController(userService) {ControllerContext = controllerContextMock.Object};

            var loginModel = new LoginModel()
                                 {
                                     Email = "testuser",
                                     Password = "testpasswordrofl"
                                 };

            var credentials = new UserCredentials()
                                  {
                                      Email = loginModel.Email,
                                      Password = loginModel.Password
                                  };

            var accessToken = new UserAccessToken("123456");

            userServiceMock.Setup(a => a.Login(credentials)).Returns(accessToken);

            var result = userController.Login(loginModel) as RedirectToRouteResult;

            Assert.IsNotNull(result, "Login Action did not yield a Redirection");
            Assert.AreEqual(result.RouteValues["controller"], "Home");
            Assert.AreEqual(result.RouteValues["action"], "Index");

            //Assert that the controller forwarded the username and password to IUserService correctly
            userServiceMock.Verify(a => a.Login(credentials), Times.Once());

            //Assert that the controller set the session state correctly
            controllerContextMock.Verify();
        }
    }
}