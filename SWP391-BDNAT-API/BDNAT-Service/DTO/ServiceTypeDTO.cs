using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Service.DTO
{
    public class ServiceTypeDTO
    {
        public int ServiceTypeId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? ServiceId { get; set; }
    }
}
