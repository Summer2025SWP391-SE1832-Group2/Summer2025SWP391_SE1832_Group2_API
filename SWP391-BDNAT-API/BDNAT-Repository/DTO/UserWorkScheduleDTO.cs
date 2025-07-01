using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class UserWorkScheduleDTO
    {
        public int UserWorkScheduleId { get; set; }

        public int? UserId { get; set; }

        public string? Title { get; set; }

        public int? WorkScheduleId { get; set; }

        public DateTime? Date { get; set; }
    }
}
