using BDNAT_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public partial class TransactionDTO
    {
        public int TransactionId { get; set; }

        public int? BookingId { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? UserId { get; set; }

        public string OrderCode { get; set; } = null!;

        public string? TransactionCode { get; set; }

        public string PaymentGateway { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string? PaymentMethod { get; set; }

        public string? PaymentUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class TotalTransactionAmountsDto
    {
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalRefundedAmount { get; set; }

        public int TotalSuccessfulTransactions { get; set; }
        public int TotalRefundedTransactions { get; set; }
    }

    public class RevenuePeriodDto
    {
        public string Label { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
