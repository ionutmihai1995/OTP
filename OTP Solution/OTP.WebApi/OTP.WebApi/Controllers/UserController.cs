using Microsoft.AspNetCore.Mvc;
using OTP.Service.Interfaces;
using OTP.WebApi.ViewModels;
using System;
using System.Threading.Tasks;

namespace OTP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserSecretKeyService _userSecretKeyService;
        private readonly IGeneratedOTPService _generatedOTPService;
        public UserController(IUserService userService, IUserSecretKeyService userSecretKeyService, IGeneratedOTPService generatedOTPService)
        {
            _userService = userService;
            _userSecretKeyService = userSecretKeyService;
            _generatedOTPService = generatedOTPService;
        }


        [HttpPost]
        [Route("checkValidity")]
        public async Task<IActionResult> CheckUserValability(CheckUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isValidUser = await _userService.IsValidUserId(model.UserId);
            if (!isValidUser)
                return BadRequest("Invalid User Id");
            //Storing token to memory. This token is used for two factor auth.
            var secretKey = await _userSecretKeyService.AddUserSecretKey(model.UserId, Guid.NewGuid().ToString("N"));
            var generatedOTP = await _generatedOTPService.AddGeneratedOTP(model.UserId, secretKey.Id);
            return Ok(new { secretKeyGuid = secretKey.Id, generatedOTP = generatedOTP.GeneratedPassword, expiredDate = generatedOTP.ExpiredDate });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isValidUser = await _userService.IsValidUserId(model.UserId);
            if (!isValidUser)
                return BadRequest("Invalid User Id");

            var isSecretKeyValid = await _userSecretKeyService.IsSecretKeyValid(model.UserId, model.SecretKeyGuid);
            if (!isSecretKeyValid)
                return BadRequest("Invalid Secret Key Guid");
            var generatedOTP = await _generatedOTPService.GetGeneratedOTP(model.SecretKeyGuid);
            if (generatedOTP.GeneratedPassword != model.GeneratedOTP)
                return BadRequest("Wrong OTP password");
            if (generatedOTP.ExpiredDate < DateTime.Now)
                return BadRequest("OTP password expired");

            //Here will be the authentication where will be returned one Bearer Authentication Token
            //which is used for future requests
            return Ok();
        }
    }
}