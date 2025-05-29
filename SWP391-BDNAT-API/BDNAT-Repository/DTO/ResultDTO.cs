using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class ResultDTO
    {
        public int ResultId { get; set; }

        public int? BookingId { get; set; }

        public int? TestParameterId { get; set; }

        public string? Value { get; set; }

        public string? Note { get; set; }
    }
}
