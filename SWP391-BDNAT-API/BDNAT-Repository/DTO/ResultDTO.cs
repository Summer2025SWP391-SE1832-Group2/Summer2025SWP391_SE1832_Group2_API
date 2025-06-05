using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class ResultDetailDTO
    {
        public int ResultId { get; set; }

        public int? BookingId { get; set; }

        public int? TestParameterId { get; set; }

        public string? Value { get; set; }

        public int? SampleId { get; set; }
    }

    public class SaveResultDetailRequest
    {
        public int BookingId { get; set; }
        public List<ResultDetailDTO> Results { get; set; } = new();
    }
}
