using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class ServiceDTO
    {
        public int ServiceId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? DurationDays { get; set; }

        public decimal? Price { get; set; }

        public int? ServiceTypeId { get; set; }

        public bool? IsAtHome { get; set; }

        public bool? IsStaffSuport { get; set; }
    }
}
