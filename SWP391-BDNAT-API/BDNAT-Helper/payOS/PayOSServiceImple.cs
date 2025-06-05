using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Helper.payOS
{
    public class PayOSServiceImple : IPayOSService
    {
        private readonly IMapper mapper;
        private readonly IConfiguration _configuration;
        private readonly PayOSService _payOSService;

        public PayOSServiceImple(IMapper mapper, IConfiguration configuration, PayOSService payOSService)
        {
            this.mapper = mapper;
            _configuration = configuration;
            _payOSService = payOSService;
        }

        public async Task<string> RequestWithPayOsAsync(Booking order, decimal amount)
        {
            // Tạo danh sách mặt hàng
            var items = new List<ItemData>
        {
        new ItemData("NẠP TIỀN VÀO HỆ THỐNG", 1, (int)amount)
        };

            // Tạo mã giao dịch (orderCode)
            long orderCode = long.Parse(DateTimeOffset.Now.ToString("yyMMddHHmmss"));

            // KHÔNG gọi lại InsertAsync(order), vì Booking đã được tạo trước rồi

            // Tạo Payment link
            var payOSModel = new PaymentData(
                orderCode: orderCode,
                amount: (int)amount,
                description: "Thanh toán đơn hàng",
                items: items,
                returnUrl: "https://summer2025swp391-se1832-group2-fe.onrender.com/",
                cancelUrl: "https://e-learning-website-bay.vercel.app/payment-fail"
            );

            var paymentUrl = await _payOSService.CreatePaymentLink(payOSModel);
            if (paymentUrl != null)
            {
                // Sau khi tạo link thanh toán thành công, tạo bản ghi Transaction nếu bạn muốn
                var transaction = new BDNAT_Repository.Entities.Transaction
                {
                    BookingId = order.BookingId, // Assuming Booking has BookingId
                    OrderCode = orderCode.ToString(),
                    PaymentGateway = "PAYOS",
                    Price = amount,
                    Status = "PENDING",
                    PaymentMethod = "BANK_TRANSFER", // hoặc để null để PayOS tự xác định
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await TransactionRepo.Instance.InsertAsync(transaction);

                return paymentUrl.checkoutUrl;
            }

            return "Create URL failed";
        }

    }
}
