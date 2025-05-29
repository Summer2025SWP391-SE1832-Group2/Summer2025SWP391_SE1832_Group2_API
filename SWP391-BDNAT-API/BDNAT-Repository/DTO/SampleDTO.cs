using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class SampleDTO
    {
        public int SampleId { get; set; }

        public int? BookingId { get; set; }

        public string? CollectedBy { get; set; }

        public DateTime? CollectedDate { get; set; }

        public string? SampleType { get; set; }

        public string? ParticipantName { get; set; }

        public string? Notes { get; set; }

        public string? Picture { get; set; }

        public string? Transport { get; set; }
    }
}
