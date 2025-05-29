using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class FeedbackDTO
    {
        public int FeedbackId { get; set; }

        public int? UserId { get; set; }

        public int? ServiceId { get; set; }

        public string? Content { get; set; }
    }
}
