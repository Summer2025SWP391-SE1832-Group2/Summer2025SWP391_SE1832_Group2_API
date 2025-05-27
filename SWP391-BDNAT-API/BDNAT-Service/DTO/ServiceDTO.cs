using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.DTO
{
    public class ServiceDTO
    {
        public int ServiceId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? DurationDays { get; set; }
    }
}
