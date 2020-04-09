using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OTP.Repository.Entities;
using OTP.Service.Interfaces;
using OTP.WebApi.Controllers;
using OTP.WebApi.ViewModels;
using System;
using System.Threading.Tasks;

namespace OTP.WebApi.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private UserController _controller;
        private Mock<IUserService> _mockUserService;
        private Mock<IUserSecretKeyService> _mockUserSecretKeyService;
        private Mock<IGeneratedOTPService> _mockGeneratedOTPService;
        [TestInitialize]
        public void Initialize_Controller()
        {
            _mockUserService = new Mock<IUserService>(MockBehavior.Strict);
            _mockUserSecretKeyService = new Mock<IUserSecretKeyService>(MockBehavior.Strict);
            _mockGeneratedOTPService = new Mock<IGeneratedOTPService>(MockBehavior.Strict);
            _controller = new UserController(_mockUserService.Object, _mockUserSecretKeyService.Object, _mockGeneratedOTPService.Object);
        }

        [TestMethod]
        public async Task CheckUserValability_InvalidModelState_Test()
        {
            //Arange
            _controller.ModelState.AddModelError("userId", "User Id is required field");

            //Act
            var result = await _controller.CheckUserValability(new CheckUserViewModel() { UserId = null });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CheckUserValability_InvalidUserId_Test()
        {
            //Arrange
            _mockUserService.Setup(i => i.IsValidUserId(It.IsAny<string>())).Returns(Task.FromResult(false));

            //Act
            var result = await _controller.CheckUserValability(new CheckUserViewModel() { UserId = "wrong" });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CheckUserValability_Valid_Test()
        {
            //Arrange
            _mockUserService.Setup(i => i.IsValidUserId(It.IsAny<string>())).Returns(Task.FromResult(true));
            var secretKey = Guid.NewGuid().ToString("N");

            _mockUserSecretKeyService.Setup(i => i.AddUserSecretKey(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new UserSecretKey() { Id = Guid.Parse("63559BC0-1FEF-4158-968E-AE4B94974F8E"), UserId = "122212MA", SecretKey = secretKey }));

            _mockGeneratedOTPService.Setup(i => i.AddGeneratedOTP(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(new GeneratedOTP() { GeneratedPassword = "abcde", ExpiredDate = DateTime.Now.AddSeconds(30)}));

            //Act
            var result = await _controller.CheckUserValability(new CheckUserViewModel() { UserId = "122212MA" });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(okResult.Value.GetType().GetProperty("secretKeyGuid").GetValue(okResult.Value, null), Guid.Parse("63559BC0-1FEF-4158-968E-AE4B94974F8E"));
            Assert.AreEqual(okResult.Value.GetType().GetProperty("generatedOTP").GetValue(okResult.Value, null), "abcde");
        }

        [TestMethod]
        public async Task Login_InvalidModelState_Test()
        {
            //Arange
            _controller.ModelState.AddModelError("userId", "User Id is required field");

            //Act
            var result = await _controller.Login(new LoginViewModel() { UserId = null });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Login_InvalidUserId_Test()
        {
            //Arrange
            _mockUserService.Setup(i => i.IsValidUserId(It.IsAny<string>())).Returns(Task.FromResult(false));

            //Act
            var result = await _controller.Login(new LoginViewModel() { UserId = "wrong" });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((result as BadRequestObjectResult).Value, "Invalid User Id");
        }

        [TestMethod]
        public async Task Login_InvalidSecretKey_Test()
        {
            //Arrange
            _mockUserService.Setup(i => i.IsValidUserId(It.IsAny<string>())).Returns(Task.FromResult(true));
            _mockUserSecretKeyService.Setup(i => i.IsSecretKeyValid(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(false));

            //Act
            var result = await _controller.Login(new LoginViewModel() { UserId = "122212", SecretKeyGuid = Guid.Empty });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((result as BadRequestObjectResult).Value, "Invalid Secret Key Guid");
        }

        [TestMethod]
        public async Task Login_WrongOTPPassword_Test()
        {
            //Arrange
            _mockUserService.Setup(i => i.IsValidUserId(It.IsAny<string>())).Returns(Task.FromResult(true));
            _mockUserSecretKeyService.Setup(i => i.IsSecretKeyValid(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _mockGeneratedOTPService.Setup(i => i.GetGeneratedOTP(It.IsAny<Guid>())).Returns(Task.FromResult(new GeneratedOTP() { GeneratedPassword = "abcde" }));

            //Act
            var result = await _controller.Login(new LoginViewModel() { UserId = "122212", SecretKeyGuid = Guid.Empty, GeneratedOTP = "wrong_pass" });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((result as BadRequestObjectResult).Value, "Wrong OTP password");
        }

        [TestMethod]
        public async Task Login_PasswordExpired_Test()
        {
            //Arrange
            _mockUserService.Setup(i => i.IsValidUserId(It.IsAny<string>())).Returns(Task.FromResult(true));
            _mockUserSecretKeyService.Setup(i => i.IsSecretKeyValid(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));

            //Password expired twenty seconds ago
            _mockGeneratedOTPService.Setup(i => i.GetGeneratedOTP(It.IsAny<Guid>())).Returns(Task.FromResult(new GeneratedOTP() { GeneratedPassword = "abcde", ExpiredDate = DateTime.Now.AddSeconds(-20) }));

            //Act
            var result = await _controller.Login(new LoginViewModel() { UserId = "122212", SecretKeyGuid = Guid.Empty, GeneratedOTP = "abcde" });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual((result as BadRequestObjectResult).Value, "OTP password expired");
        }

        [TestMethod]
        public async Task Login_Ok_Test()
        {
            //Arrange
            _mockUserService.Setup(i => i.IsValidUserId(It.IsAny<string>())).Returns(Task.FromResult(true));
            _mockUserSecretKeyService.Setup(i => i.IsSecretKeyValid(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));

            //Password expired twenty seconds ago
            _mockGeneratedOTPService.Setup(i => i.GetGeneratedOTP(It.IsAny<Guid>())).Returns(Task.FromResult(new GeneratedOTP() { GeneratedPassword = "abcde", ExpiredDate = DateTime.Now.AddSeconds(20) }));

            //Act
            var result = await _controller.Login(new LoginViewModel() { UserId = "122212", SecretKeyGuid = Guid.Empty, GeneratedOTP = "abcde" });

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
