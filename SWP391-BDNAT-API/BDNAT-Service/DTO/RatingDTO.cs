using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.DTO
{
    public class RatingDTO
    {
        public int RatingId { get; set; }

        public string? Content { get; set; }

        public int? Vote { get; set; }

        public int? BookingId { get; set; }

        public int CreateBy { get; set; }
    }
}
