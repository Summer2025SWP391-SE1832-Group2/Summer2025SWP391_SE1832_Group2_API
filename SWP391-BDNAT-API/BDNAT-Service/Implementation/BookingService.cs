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
            bool isDuplicate = await BookingRepo.Instance.IsScheduleDuplicatedAsync(
                bookingDto.CollectionDate,
                bookingDto.Time,
                bookingDto.Location
            );

            if (isDuplicate)
            {
                throw new InvalidOperationException("Lịch hẹn bị trùng. Vui lòng chọn thời gian khác.");
            }

            // Map booking
            var booking = _mapper.Map<Booking>(bookingDto);
            booking.BookingDate = DateTime.Now;
            var service = await ServiceRepo.Instance.GetByIdAsync(bookingDto.ServiceId);
            booking.PreferredDate = bookingDto.CollectionDate.AddDays((double)service.DurationDays);
            long orderCode = long.Parse(DateTimeOffset.Now.ToString("yyMMddHHmmss"));
            //Update OrderCode of Booking
            booking.OrderCode = orderCode;
            booking.PaymentStatus = "Chưa thanh toán";
            // Insert booking
            var bookingCreated = await BookingRepo.Instance.InsertAsync(booking);
            if (!bookingCreated)
                return null;

            // Insert schedule
            var schedule = new SampleCollectionSchedule
            {
                BookingId = booking.BookingId,
                CollectionDate = bookingDto.CollectionDate,
                Time = bookingDto.Time,
                Location = bookingDto.Location,
                Status = "Đang chờ"
            };

            var scheduleCreated = await SampleCollectionScheduleRepo.Instance.InsertAsync(schedule);
            if (!scheduleCreated)
                return null;
            decimal amount = 0;
            if (bookingDto.Method.Equals("Delivery"))
            {
                amount = (decimal)(service.Price + 3000);
            }
            else if (booking.Method.Equals("SupportAtHome"))
            {
                amount = (decimal)(service.Price + 3000 + ((decimal)service.Price * 0.2m));
            }
            else
            {
                amount = (decimal)(service.Price);
            }

            
            var paymentUrl = await _payOSService.RequestWithPayOsAsync(booking, amount,orderCode);

            // Kiểm tra nếu không có paymentUrl thì dừng lại
            if (string.IsNullOrEmpty(paymentUrl))
                return null;

            // BƯỚC 5: Tạo Transaction nếu có paymentUrl
            
            var transaction = new Transaction
            {
                BookingId = booking.BookingId,
                UserId = booking.UserId,
                Description = "Thanh toán đơn hàng",
                Price = amount,
                OrderCode = orderCode,
                PaymentGateway = "PayOS",
                Status = "Đang chờ",
                PaymentUrl = paymentUrl,
                CreatedAt = DateTime.Now
            };
            await TransactionRepo.Instance.InsertAsync(transaction);

            // Trả về link thanh toán
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
                Status = "Đang chờ",
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
            var list = await BookingRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<BookingDisplayDTO>(x)).ToList();
        }

        public async Task<List<BookingSampleDTO>> GetAllBookingWithSampleAsync()
        {
            var list = await BookingRepo.Instance.GetAllBookingWithSample();
            return list.Select(x => _mapper.Map<BookingSampleDTO>(x)).ToList();
        }

        public async Task<List<BookingScheduleDTO>> GetAllBookingWithScheduleAsync()
        {
            var list = await BookingRepo.Instance.GetAllBookingWithSchedule();
            return list.Select(x => _mapper.Map<BookingScheduleDTO>(x)).ToList();
        }

        public async Task<BookingDisplayDetailDTO?> GetBookingByIdAsync(int id)
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

        public async Task<bool> UpdateBookingAsync(BookingRequestDTO booking)
        {
            var map = _mapper.Map<Booking>(booking);
            return await BookingRepo.Instance.UpdateAsync(map);
        }
    }

}
