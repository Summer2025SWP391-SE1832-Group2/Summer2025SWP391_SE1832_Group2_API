using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO.Auth
{
    public class Token
    {
        public string AccessTokenToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiredAt { get; set; }
    }

    public class FCMToken
    {
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
    }
}
