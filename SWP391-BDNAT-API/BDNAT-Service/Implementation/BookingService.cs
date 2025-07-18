using AutoMapper;
using BDNAT_Repository.Entities;
using BDNAT_Repository.Implementation;
using BDNAT_Repository.DTO;
using BDNAT_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BDNAT_Helper.payOS;

namespace BDNAT_Service.Implementation
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;

        private readonly IPayOSService _payOSService;

        public BookingService(IMapper mapper, IPayOSService payOSService)
        {
            _mapper = mapper;
            _payOSService = payOSService;
        }

        public async Task<bool> CheckPending(int userId)
        {
            var bookings = await BookingRepo.Instance.GetBookingByUserIdAsync(userId);
            return bookings.Any(b => b.Status == "Pending");
        }

        public async Task<string?> CreateBookingAsync(BookingRequestDTO bookingDto)
        {
            // 1. Kiểm tra trùng lịch
            bool isDuplicate = await BookingRepo.Instance.IsScheduleDuplicatedAsync(
                bookingDto.CollectionDate,
                bookingDto.Time,
                bookingDto.Location,
                bookingDto.UserId
            );

            if (isDuplicate)
                throw new InvalidOperationException("Lịch hẹn bị trùng. Vui lòng chọn thời gian khác.");

            // 2. Kiểm tra đã có booking "Đang chờ xử lý"
            var bookings = await BookingRepo.Instance.GetBookingByUserIdAsync(bookingDto.UserId);
            bool hasPending = bookings.Any(b => b.Status == "Đang chờ xử lý");

            // 3. Map booking & tính toán
            var booking = _mapper.Map<Booking>(bookingDto);
            booking.BookingDate = DateTime.Now;
            var service = await ServiceRepo.Instance.GetByIdAsync(bookingDto.ServiceId);
            booking.PreferredDate = bookingDto.CollectionDate.AddDays((double)service.DurationDays);
            long orderCode = long.Parse(DateTimeOffset.Now.ToString("yyMMddHHmmss"));
            booking.OrderCode = orderCode;
            booking.Status = "Đang chờ xử lý";
            booking.PaymentStatus = "Chưa thanh toán";

            // 4. Insert Booking
            var bookingCreated = await BookingRepo.Instance.InsertAsync(booking);
            if (!bookingCreated) return null;

            // 5. Insert Schedule
            var schedule = new SampleCollectionSchedule
            {
                BookingId = booking.BookingId,
                CollectionDate = bookingDto.CollectionDate,
                Time = bookingDto.Time,
                Location = bookingDto.Location,
                Status = "Pending"
            };
            var scheduleCreated = await SampleCollectionScheduleRepo.Instance.InsertAsync(schedule);
            if (!scheduleCreated) return null;

            // Nếu là phương thức tự thu mẫu => tạo thêm ShippingOrder
            if (bookingDto.Method == "TU_THU_MAU")
            {
                var user = await UserRepo.Instance.GetById(bookingDto.UserId);
                var shippingOrder = new ShippingOrder
                {
                    BookingId = booking.BookingId,
                    Receiver = user.FullName, // Hoặc lấy tên thực từ UserRepo nếu muốn
                    Address = bookingDto.Location,
                    ShipperId = null, // Gán sau nếu có phân công
                    Status = "Chờ giao hàng",
                    CreateAt = DateTime.Now.AddDays(1),
                    UpdateAt = null
                };

                var shippingCreated = await ShippingOrderRepo.Instance.InsertAsync(shippingOrder);
                if (!shippingCreated) return null;
            }

            // 6. Tính tiền
            decimal amount;
            if (bookingDto.Method == "Delivery")
                amount = (decimal)(service.Price + 3000);
            else if (bookingDto.Method == "SupportAtHome")
                amount = (decimal)(service.Price + 3000 + (service.Price * 0.2m));
            else
                amount = (decimal)service.Price;

            // 7. Tạo payment
            var paymentUrl = await _payOSService.RequestWithPayOsAsync(booking, amount, orderCode);
            if (string.IsNullOrEmpty(paymentUrl)) return null;

            // 8. Ghi transaction
            var transaction = new Transaction
            {
                BookingId = booking.BookingId,
                UserId = booking.UserId,
                Description = "Thanh toán đơn hàng",
                Price = amount,
                OrderCode = orderCode,
                PaymentGateway = "PayOS",
                Status = "Chưa thanh toán",
                PaymentUrl = paymentUrl,
                CreatedAt = DateTime.Now
            };
            await TransactionRepo.Instance.InsertAsync(transaction);

            // 9. Trả kết quả
            return paymentUrl;
        }

        public async Task<string?> RegeneratePaymentQrAsync(int bookingId)
        {
            var booking = await BookingRepo.Instance.GetById(bookingId);
            if (booking == null)
                throw new InvalidOperationException("Không tìm thấy đơn đặt lịch.");

            if (!string.Equals(booking.PaymentStatus, "Chưa thanh toán", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Đơn hàng đã được thanh toán. Không thể tạo lại mã QR.");

            var service = await ServiceRepo.Instance.GetByIdAsync(booking.ServiceId);
            if (service == null)
                throw new InvalidOperationException("Không tìm thấy dịch vụ.");

            // Tạo mã đơn hàng mới
            long orderCode = long.Parse(DateTimeOffset.Now.ToString("yyMMddHHmmss"));

            // Cập nhật lại OrderCode của booking
            booking.OrderCode = orderCode;
            bool updated = await BookingRepo.Instance.UpdateAsync(booking);
            if (!updated)
                throw new InvalidOperationException("Không thể cập nhật mã đơn hàng mới cho booking.");

            // Tính lại số tiền
            decimal amount = 0;
            if (booking.Method == "Delivery")
            {
                amount = (decimal)(service.Price + 3000);
            }
            else if (booking.Method == "SupportAtHome")
            {
                amount = (decimal)(service.Price + 3000 + ((decimal)service.Price * 0.2m));
            }
            else
            {
                amount = (decimal)(service.Price);
            }

            // Gọi API tạo mã thanh toán
            var paymentUrl = await _payOSService.RequestWithPayOsAsync(booking, amount, orderCode);
            if (string.IsNullOrEmpty(paymentUrl))
                return null;

            // Tạo giao dịch mới
            var transaction = new Transaction
            {
                BookingId = booking.BookingId,
                UserId = booking.UserId,
                Description = "Tạo lại mã thanh toán",
                Price = amount,
                OrderCode = orderCode,
                PaymentGateway = "PayOS",
                Status = "Chưa thanh toán",
                PaymentUrl = paymentUrl,
                CreatedAt = DateTime.Now
            };

            await TransactionRepo.Instance.InsertAsync(transaction);

            return paymentUrl;
        }


        public async Task<bool> DeleteBookingAsync(int id)
        {
            return await BookingRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<BookingDisplayDTO>> GetAllBookingsAsync()
        {
            var list = await BookingRepo.Instance.GetAllBookingWithSchedule();
            var result = list.Select(x =>
            {
                var schedule = x.SampleCollectionSchedules?.FirstOrDefault();

                return new BookingDisplayDTO
                {
                    BookingId = x.BookingId,
                    UserId = x.UserId,
                    ServiceId = x.ServiceId,
                    BookingDate = x.BookingDate,
                    Status = x.Status,
                    PaymentStatus = x.PaymentStatus,
                    PreferredDate = x.PreferredDate,
                    Method = x.Method,
                    CollectionDate = schedule?.CollectionDate,
                    Time = schedule?.Time,
                    Location = schedule?.Location ?? "",
                    hasSubmittedRating = x.Ratings.Any(),
                };
            }).ToList();

            return result;
        }

        public async Task<List<BookingSampleDTO>> GetAllBookingWithSampleAsync()
        {
            var list = await BookingRepo.Instance.GetAllBookingWithSample();
            return list.Select(x => _mapper.Map<BookingSampleDTO>(x)).ToList();
        }

        public async Task<List<BookingScheduleDTO>> GetAllBookingWithScheduleAsync()
        {
            var data = await BookingRepo.Instance.GetAllBookingWithSchedule();

            var result = data.Select(b => new BookingScheduleDTO
            {
                BookingId = b.BookingId,
                UserId = b.UserId,
                FullName = b.User?.FullName,
                BookingDate = b.BookingDate,
                Status = b.Status,
                PaymentStatus = b.PaymentStatus,
                PreferredDate = b.PreferredDate,
                Method = b.Method,
                SampleCollectionSchedules = b.SampleCollectionSchedules?.Select(s => new SampleCollectionScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    BookingId = s.BookingId,
                    CollectorId = s.CollectorId,
                    CollectorName = s.Collector?.FullName,
                    CollectionDate = s.CollectionDate,
                    Time = s.Time,
                    Location = s.Location,
                    Status = s.Status
                }).ToList()
            }).ToList();

            return result;
        }

        public async Task<BookingDisplayDetailDTO> GetBookingByIdAsync(int id)
        {
            var data = await BookingRepo.Instance.GetBookingByIdAsync(id);
            if (data == null) return null;

            return data;
        }


        public async Task<List<BookingDisplayDTO>> GetBookingByUserIdAsync(int id)
        {
            var list = await BookingRepo.Instance.GetBookingByUserIdAsync(id);

            var result = list.Select(x =>
            {
                var schedule = x.SampleCollectionSchedules?.FirstOrDefault();

                return new BookingDisplayDTO
                {
                    BookingId = x.BookingId,
                    UserId = x.UserId,
                    BookingDate = x.BookingDate,
                    Status = x.Status,
                    PaymentStatus = x.PaymentStatus,
                    PreferredDate = x.PreferredDate,
                    Method = x.Method,
                    CollectionDate = schedule?.CollectionDate,
                    Time = schedule?.Time,
                    Location = schedule?.Location ?? ""
                };
            }).ToList();

            return result;
        }

        public async Task<bool> UpdateBookingAsync(BookingDisplayDTO booking)
        {
            var map = _mapper.Map<Booking>(booking);
            return await BookingRepo.Instance.UpdateAsync(map);
        }

        public async Task<List<BookingScheduleDTO>> GetBookingsByCollectorAsync(int collectorId)
        {
            var bookings = await BookingRepo.Instance.GetBookingsByCollectorIdAsync(collectorId);

            var result = bookings.Select(b => new BookingScheduleDTO
            {
                BookingId = b.BookingId,
                UserId = b.UserId,
                FullName = b.User?.FullName,
                BookingDate = b.BookingDate,
                Status = b.Status,
                PaymentStatus = b.PaymentStatus,
                PreferredDate = b.PreferredDate,
                Method = b.Method,
                SampleCollectionSchedules = b.SampleCollectionSchedules?.Select(s => new SampleCollectionScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    BookingId = s.BookingId,
                    CollectorId = s.CollectorId,
                    CollectorName = s.Collector?.FullName,
                    CollectionDate = s.CollectionDate,
                    Time = s.Time,
                    Location = s.Location,
                    Status = s.Status
                }).ToList()
            }).ToList();

            return result;
        }

    }
}
