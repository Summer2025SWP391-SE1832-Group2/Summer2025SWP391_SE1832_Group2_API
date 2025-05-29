using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class BookingDTO
    {
        public int BookingId { get; set; }

        public int? ServiceTypeId { get; set; }

        public int? UserId { get; set; }

        public DateTime? BookingDate { get; set; }

        public string? SampleMethod { get; set; }

        public string? Status { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime? PreferredDate { get; set; }

        public string? Result { get; set; }
    }
}
