using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class KitOrderDTO
    {
        public int KitOrderId { get; set; }

        public int? TestKitId { get; set; }

        public int? BookingId { get; set; }

        public int? ShippingId { get; set; }

        public string? Status { get; set; }
    }
}
