using System;
using System.Collections.Generic;
using System.Text;

namespace OTP.Repository.Entities
{
    public class GeneratedOTP : IdentifiableEntity
    {
        public string UserId { get; set; }
        public Guid SecretKeyGuid { get; set; }
        public string GeneratedPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
