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

        public async Task<string?> CreateBookingAsync(BookingDTO bookingDto)
        {
            var booking = _mapper.Map<Booking>(bookingDto);

            // Insert booking
            var bookingCreated = await BookingRepo.Instance.InsertAsync(booking);
            if (!bookingCreated)
                return null;

            // Insert schedule
            var schedule = new SampleCollectionSchedule
            {
                BookingId = booking.BookingId,
                CollectionDate = bookingDto.PreferredDate ?? DateTime.Now,
                Time = bookingDto.Time,
                Location = bookingDto.Location,
                Status = "Pending"
            };

            var scheduleCreated = await SampleCollectionScheduleRepo.Instance.InsertAsync(schedule);
            if (!scheduleCreated)
                return null;
            var service = await ServiceRepo.Instance.GetByIdAsync(bookingDto.ServiceId);
            // Gọi dịch vụ tạo link thanh toán
            var amount = service.Price ?? 0; // Bạn cần thêm Amount vào DTO nếu chưa có
            var paymentUrl = await _payOSService.RequestWithPayOsAsync(booking, amount);

            return paymentUrl;
        }



        public async Task<bool> DeleteBookingAsync(int id)
        {
            return await BookingRepo.Instance.DeleteAsync(id);
        }

        public async Task<List<BookingDTO>> GetAllBookingsAsync()
        {
            var list = await BookingRepo.Instance.GetAllAsync();
            return list.Select(x => _mapper.Map<BookingDTO>(x)).ToList();
        }

        public async Task<BookingDTO> GetBookingByIdAsync(int id)
        {
            return _mapper.Map<BookingDTO>(await BookingRepo.Instance.GetByIdAsync(id));
        }

        public async Task<bool> UpdateBookingAsync(BookingDTO booking)
        {
            var map = _mapper.Map<Booking>(booking);
            return await BookingRepo.Instance.UpdateAsync(map);
        }
    }

}
