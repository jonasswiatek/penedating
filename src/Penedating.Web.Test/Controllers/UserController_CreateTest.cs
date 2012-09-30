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
using Penedating.Web.Controllers;
using Penedating.Web.Models;

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
            var createModel = new UserCreateModel();

            var result = userController.Create(createModel) as ViewResult;

            //Verify that the controller does not try to call login on IUserService
            userServiceMock.Verify(a => a.Create(createModel.Username, createModel.Password), Times.Never());

            Assert.IsNotNull(result, "Login Action did not yield a ViewResult");
            Assert.AreSame(createModel, result.Model, "Controller did not forward the defective view");
        }

        [Test]
        public void Create_TestExistingUser()
        {
            var userServiceMock = new Mock<IUserService>();
            var userService = userServiceMock.Object;

            var userController = new UserController(userService);
            var createModel = new UserCreateModel
                                  {
                                      Username = "test-user",
                                      Password = "pssword",
                                      StreetAddress = "rofl",
                                      City = "mao",
                                      ZipCode = 1000
                                  };

            userServiceMock.Setup(a => a.Create(createModel.Username, createModel.Password)).Throws(new UserExistsException());
            var viewResult = userController.Create(createModel) as ViewResult;

            Assert.IsNotNull(viewResult, "Controller did not return a ViewResult");
            Assert.AreSame(createModel, viewResult.Model);
            userServiceMock.Verify(a => a.Create(createModel.Username, createModel.Password));
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

            var createModel = new UserCreateModel
            {
                Username = "test-user",
                Password = "pssword",
                StreetAddress = "rofl",
                City = "mao",
                ZipCode = 1000
            };

            var testUser = new User("userid-12345", createModel.Username);
            userServiceMock.Setup(a => a.Create(createModel.Username, createModel.Password)).Returns(testUser);
            userServiceMock.Setup(a => a.Login(createModel.Username, createModel.Password)).Returns(testUser);

            var viewResult = userController.Create(createModel) as RedirectToRouteResult;

            Assert.IsNotNull(viewResult, "Controller did not return a RedirectToRouteResult");

            userServiceMock.Verify(a => a.Create(createModel.Username, createModel.Password));
            userServiceMock.Verify(a => a.Update(testUser));

            Assert.IsNotNull(testUser.Address, "Controller did not initialize user property on object returned from IUserService");
            Assert.AreEqual(testUser.Address.Street, createModel.StreetAddress);
            Assert.AreEqual(testUser.Address.City, createModel.City);
            Assert.AreEqual(testUser.Address.ZipCode, createModel.ZipCode);
        }
    }
}
