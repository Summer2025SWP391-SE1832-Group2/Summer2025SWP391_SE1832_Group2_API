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

        public async Task<string?> CreateBookingAsync(BookingRequestDTO bookingDto)
        {
            // Map booking
            var booking = _mapper.Map<Booking>(bookingDto);
            booking.BookingDate = DateTime.Now;
            booking.PreferredDate = bookingDto.CollectionDate.AddDays(1);
            long orderCode = long.Parse(DateTimeOffset.Now.ToString("yyMMddHHmmss"));
            booking.OrderCode = orderCode;
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
                Status = "Pending"
            };

            var scheduleCreated = await SampleCollectionScheduleRepo.Instance.InsertAsync(schedule);
            if (!scheduleCreated)
                return null;
            decimal amount = 0;
            var service = await ServiceRepo.Instance.GetByIdAsync(bookingDto.ServiceId);
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
                Status = "PENDING",
                PaymentUrl = paymentUrl,
                CreatedAt = DateTime.Now
            };
            await TransactionRepo.Instance.InsertAsync(transaction);

            // Trả về link thanh toán
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

        public async Task<List<BookingDisplayDTO>> GetAllBookingWithScheduleAsync()
        {
            var list = await BookingRepo.Instance.GetAllBookingWithSchedule();
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

        public async Task<BookingDisplayDTO> GetBookingByIdAsync(int id)
        {
            return _mapper.Map<BookingDisplayDTO>(await BookingRepo.Instance.GetByIdAsync(id));
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
