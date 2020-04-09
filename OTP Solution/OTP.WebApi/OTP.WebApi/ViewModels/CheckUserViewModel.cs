using System.ComponentModel.DataAnnotations;

namespace OTP.WebApi.ViewModels
{
    public class CheckUserViewModel
    {
        [Required]
        public string UserId { get; set; }
    }
}
