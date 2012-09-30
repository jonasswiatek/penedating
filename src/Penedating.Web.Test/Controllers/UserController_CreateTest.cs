using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using Penedating.Service.Model.Exceptions;
using Penedating.Web.App_Start;
using Penedating.Web.Controllers;
using Penedating.Web.Models;
using Penedating.Web.Security;

namespace Penedating.Web.Test.Controllers
{
    [TestFixture]
    public class UserController_CreateTest
    {
        [Test]
        public void Create_TestFormValidation()
        {
            var userServiceMock = new Mock<IUserService>();
            var userService = userServiceMock.Object;

            var userController = new UserController(userService);
            userController.ViewData.ModelState.AddModelError("rofl", "nao"); //This simulates any validation error

            var createModel = new CreateViewModel();
            var userCredentials = new UserCredentials()
                                      {
                                          Email = createModel.Email,
                                          Password = createModel.Password
                                      };

            var result = userController.Index(createModel) as ViewResult;
            
            //Verify that the controller does not try to call login on IUserService
            userServiceMock.Verify(a => a.Create(userCredentials), Times.Never());

            Assert.IsNotNull(result, "Login Action did not yield a ViewResult");
            Assert.AreSame(createModel, result.Model, "Controller did not forward the defective view");
        }

        [Test]
        public void Create_TestExistingUser()
        {
            var userServiceMock = new Mock<IUserService>();
            var userService = userServiceMock.Object;

            var userController = new UserController(userService);
            var createModel = new CreateViewModel
                                  {
                                      Username = "test-user",
                                      Password = "pssword",
                                      StreetAddress = "rofl",
                                      City = "mao",
                                      ZipCode = 1000
                                  };

            var credentials = new UserCredentials()
                                  {
                                      Email = createModel.Email,
                                      Password = createModel.Password
                                  };

            userServiceMock.Setup(a => a.Create(credentials)).Throws(new UserExistsException());
            var viewResult = userController.Index(createModel) as ViewResult;

            Assert.IsNotNull(viewResult, "Controller did not return a ViewResult");
            Assert.AreSame(createModel, viewResult.Model);
            userServiceMock.Verify(a => a.Create(credentials));
        }

        [Test]
        public void Create_TestSuccess()
        {
            var userServiceMock = new Mock<IUserService>();
            var userService = userServiceMock.Object;

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupSet(a => a.HttpContext.Session["UserState"] = It.IsAny<UserState>()).Verifiable("Controllre did not set session state");

            var userController = new UserController(userService);
            userController.ControllerContext = controllerContextMock.Object;

            var createModel = new CreateViewModel
            {
                Username = "test-user",
                Password = "pssword",
                StreetAddress = "rofl",
                City = "mao",
                ZipCode = 1000
            };

            var credentials = new UserCredentials()
                                  {
                                      Email = createModel.Email,
                                      Password = createModel.Password
                                  };

            var accessToken = new UserAccessToken("123456");

            userServiceMock.Setup(a => a.Create(credentials)).Returns(accessToken);
            userServiceMock.Setup(a => a.Login(credentials)).Returns(accessToken);

            var viewResult = userController.Index(createModel) as RedirectToRouteResult;

            Assert.IsNotNull(viewResult, "Controller did not return a RedirectToRouteResult");

            var userProfile = new UserProfile()
                                  {
                                      Username = createModel.Username,
                                      Address = new Address()
                                                    {
                                                        Street = createModel.StreetAddress,
                                                        City = createModel.City,
                                                        ZipCode = createModel.ZipCode
                                                    }
                                  };

            userServiceMock.Verify(a => a.Create(credentials));
            userServiceMock.Verify(a => a.UpdateProfile(accessToken, userProfile));
        }
    }
}
