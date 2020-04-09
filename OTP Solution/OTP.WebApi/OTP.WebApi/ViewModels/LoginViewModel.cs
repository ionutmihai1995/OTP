using System;
using System.ComponentModel.DataAnnotations;

namespace OTP.WebApi.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string GeneratedOTP { get; set; }
        [Required]
        public Guid SecretKeyGuid { get; set; }
    }
}
