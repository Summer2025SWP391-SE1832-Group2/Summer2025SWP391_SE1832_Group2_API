using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class SampleCollectionScheduleDTO
    {
        public int ScheduleId { get; set; }

        public int BookingId { get; set; }

        public int? CollectorId { get; set; }
        public string? CollectorName { get; set; }
        public DateTime CollectionDate { get; set; }

        public string? Time { get; set; }

        public string Location { get; set; } = null!;

        public string? Status { get; set; }
    }
}
