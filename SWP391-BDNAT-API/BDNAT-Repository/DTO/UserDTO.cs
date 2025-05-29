using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public partial class UserDTO
    {
        public int UserId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? PasswordHash { get; set; }

        public string? Role { get; set; }

        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
