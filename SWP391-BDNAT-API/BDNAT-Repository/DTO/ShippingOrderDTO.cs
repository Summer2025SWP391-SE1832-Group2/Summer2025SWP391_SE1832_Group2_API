using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class ShippingOrderDTO
    {
        public int ShippingId { get; set; }

        public string? Receiver { get; set; }

        public string? Address { get; set; }

        public int? ShipperId { get; set; }

        public string? Status { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int? BookingId { get; set; }
    }
}
