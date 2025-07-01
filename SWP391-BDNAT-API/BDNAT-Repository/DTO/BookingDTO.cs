using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class BookingRequestDTO
    {
        public int BookingId { get; set; }

        public int? ServiceId { get; set; }

        public int UserId { get; set; }

        public DateTime? BookingDate { get; set; }
        public string? Status { get; set; }
        public bool BuyKit { get; set; }

        public string Method { get; set; } = null!;

        public string? PaymentStatus { get; set; }

        public DateTime? PreferredDate { get; set; }

        public string? Result { get; set; }

        public DateTime CollectionDate { get; set; }

        public string? Time { get; set; }

        public string Location { get; set; } = null!;
    }

    public class BookingDisplayDTO
    {
        public int BookingId { get; set; }

        public int? UserId { get; set; }
        public int? ServiceId { get; set; }

        public DateTime? BookingDate { get; set; }

        public string? Status { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime? PreferredDate { get; set; }

        public string Method { get; set; } = null!;

        public DateTime? CollectionDate { get; set; }

        public string? Time { get; set; }

        public string Location { get; set; } = null!;
    }

    public class BookingDisplayDetailDTO
    {
        public int BookingId { get; set; }

        public int? UserId { get; set; }

        public string? FullName { get; set; }

        public DateTime? BookingDate { get; set; }

        public string? Status { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime? PreferredDate { get; set; }

        public string Method { get; set; } = null!;

        public string? FinalResult { get; set; }

        public DateTime? CollectionDate { get; set; }

        public string? Time { get; set; }

        public string Location { get; set; } = null!;

        public List<ResultDetailDTO>? ResultDetails { get; set; }
    }

    public class BookingScheduleDTO
    {
        public int BookingId { get; set; }

        public int? UserId { get; set; }
        public string? FullName { get; set; }

        public DateTime? BookingDate { get; set; }

        public string? Status { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime? PreferredDate { get; set; }

        public string Method { get; set; } = null!;

        public List<SampleCollectionScheduleDTO>? SampleCollectionSchedules { get; set; }
    }

    public class BookingSampleDTO
    {
        public int BookingId { get; set; }

        public int? ServiceId { get; set; }

        public int? UserName { get; set; }

        public DateTime? BookingDate { get; set; }

        public string? SampleMethod { get; set; }

        public string? Status { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime? PreferredDate { get; set; }

        public string? FinalResult { get; set; }

        public List<SampleDTO>? Samples { get; set; }
    }
}
