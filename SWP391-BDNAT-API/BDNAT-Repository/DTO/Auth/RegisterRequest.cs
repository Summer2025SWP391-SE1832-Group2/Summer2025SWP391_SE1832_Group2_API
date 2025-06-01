using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO.Auth
{
    public class RegisterRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class VerificationPending
    {
        public string Email { get; set; }
        public string VerifyCode { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
